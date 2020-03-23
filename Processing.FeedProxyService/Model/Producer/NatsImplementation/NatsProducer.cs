using Center.Processing.Framework.NatsStreaming;
using Center.Processing.Framework.NatsStreaming.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Processing.FeedProxyService.Model.Consumer.NatsImplementation;
using Processing.FeedProxyService.Model.Producer.Interfaces;
using System.Threading.Tasks;

namespace Processing.FeedProxyService.Model.Producer.NatsImplementation
{
    public class NatsProducer : IProducer
    {
        private const string CLIENT_ID = "feed-proxy-producer";

        private readonly INatsStreamingProducer _natsStreamingProducer;
        private readonly ILogger<NatsConsumer> _logger;

        public string Topic { get; }

        public string Environment { get; }

        public NatsProducer(
            string topic,
            string environment,
            INatsStreamingProducerFactory producerFactory,
            ILogger<NatsConsumer> logger = null)
        {
            Topic = topic;
            Environment = environment;

            _logger = logger ?? new NullLogger<NatsConsumer>();

            var producerConfiguration = new NatsStreamingSubscriptionConfigurationBuilder()
                .WithSubscriptionName(topic)
                .Build();

            var clientId = $"{CLIENT_ID}-{Environment}-{Topic.Replace(".", "-")}";

            _natsStreamingProducer = producerFactory.Create(producerConfiguration, clientId);
        }

        public void Connect()
        {
            _natsStreamingProducer.Connect();
            _logger.LogInformation($"Producer {Environment} connected on topic {Topic}");
        }

        public async Task SendAsync(byte[] data)
        {
            _logger.LogInformation($"Send message {Environment} - {Topic}");
            await _natsStreamingProducer.SendAsync(data);
        }

        public void Dispose()
        {
            _natsStreamingProducer?.Dispose();
        }
    }
}
