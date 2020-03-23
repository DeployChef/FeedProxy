using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Processing.FeedProxyService.Configs;
using Processing.FeedProxyService.Model.Consumer.Interfaces;
using Processing.FeedProxyService.Model.Producer.Interfaces;
using Prometheus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Processing.FeedProxyService.FeedProxies
{
    public class FeedProxy : IFeedProxy
    {
        private readonly IConsumerFactory _consumerFactory;
        private readonly IProducerFactory _producerFactory;
        private string _topic;
        private readonly ILogger<FeedProxy> _logger;
        private IConsumer _consumer;
        private readonly List<IProducer> _producers = new List<IProducer>();

        public int ConnectedProducersCount => _producers.Count;

        public FeedProxy(
            IConsumerFactory consumerFactory,
            IProducerFactory producerFactory,
            ILogger<FeedProxy> logger = null)
        {
            _consumerFactory = consumerFactory ?? throw new ArgumentNullException(nameof(consumerFactory));
            _producerFactory = producerFactory ?? throw new ArgumentNullException(nameof(producerFactory));
            _logger = logger ?? new NullLogger<FeedProxy>();
        }

        public bool Run(string topic, ConsumerConfig consumeConfiguration, IEnumerable<ProducerConfig> produceConfigurations)
        {
            _topic = topic;
            _logger.LogInformation($"Starting feed proxy for topic {_topic}");

            try
            {
                _consumer = _consumerFactory.Create(_topic, consumeConfiguration);
                _consumer.Connect();
                _logger.LogInformation($"Connect consumer {_consumer.Environment}, topic {_topic}");
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Exception while connect consumer {_consumer.Environment}.");
                return false;
            }

            foreach (var producerConfiguration in produceConfigurations)
            {
                var producer = _producerFactory.Create(_topic, producerConfiguration);
                try
                {
                    producer.Connect();
                    _producers.Add(producer);
                    _logger.LogInformation($"Connect producer {producer.Environment}, topic {_topic}");
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"Exception while connect producer {producer.Environment}.");
                }
            }

            if (!_producers.Any())
            {
                _logger.LogWarning($"Doesn't have any connected producers for topic {_topic}, feed proxy not started!");
                return false;
            }

            _consumer.Subscribe(Handler);
            _logger.LogInformation($"Subscribe consumer {_consumer.Environment}, topic {_topic}");
            return true;
        }

        private async Task Handler(byte[] arg)
        {
            _logger.LogInformation($"Receive from {_consumer.Environment}, topic {_topic}");
            ReceiveMetrics(_topic, _consumer.Environment);

            foreach (var producer in _producers)
            {
                try
                {
                    _logger.LogInformation($"Send to {producer.Environment}, topic {_topic}");

                    await producer.SendAsync(arg);

                    SendMetrics(_topic, producer.Environment);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"Exception while send {producer.Environment} on topic {_topic}.");
                    DropMetrics(_topic, producer.Environment);
                }
            }
        }

        private void DropMetrics(string topic, string environment)
        {
            var topicKey = topic.Replace(".", "_");
            var counter = Metrics.CreateCounter($"{environment}_{topicKey}_feedproxy_drop_total", "Drop messages metrics from feed proxy",
                new CounterConfiguration
                {
                    LabelNames = new[] { "environment", "topic" }
                });
            counter.Labels(environment, _topic).Inc();
        }

        private void SendMetrics(string topic, string environment)
        {
            var topicKey = topic.Replace(".", "_");
            var counter = Metrics.CreateCounter($"{environment}_{topicKey}_feedproxy_send_total", "Send metrics from feed proxy",
                new CounterConfiguration
                {
                    LabelNames = new[] { "environment", "topic" }
                });
            counter.Labels(environment, _topic).Inc();
        }

        private void ReceiveMetrics(string topic, string environment)
        {
            var topicKey = topic.Replace(".", "_");
            var counter = Metrics.CreateCounter($"{environment}_{topicKey}_feedproxy_receive_total", "Receive metrics from feed proxy",
                new CounterConfiguration
                {
                    LabelNames = new[] { "environment", "topic" }
                });
            counter.Labels(environment, _topic).Inc();
        }

        public void Dispose()
        {
            _logger.LogInformation($"Dispose feed proxy for topic {_topic}");
            _consumer?.Dispose();
            _producers.ForEach(c => c.Dispose());
        }
    }
}
