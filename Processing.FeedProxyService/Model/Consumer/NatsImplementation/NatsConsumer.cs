using Center.Processing.Framework.NatsStreaming;
using Center.Processing.Framework.NatsStreaming.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Processing.FeedProxyService.Model.Consumer.Interfaces;
using System;
using System.Threading.Tasks;

namespace Processing.FeedProxyService.Model.Consumer.NatsImplementation
{
    public class NatsConsumer : IConsumer
    {
        private const string CLIENT_ID = "feed-proxy-consumer";

        private readonly INatsStreamingConsumer _natsStreamingConsumer;
        private readonly ILogger<NatsConsumer> _logger;

        public string Topic { get; }

        public string Environment { get; }

        public NatsConsumer(
            string topic,
            string environment,
            INatsStreamingConsumerFactory consumerFactory,
            ILogger<NatsConsumer> logger = null)
        {
            Topic = topic;
            Environment = environment;
            _logger = logger ?? new NullLogger<NatsConsumer>();

            var clientId = $"{CLIENT_ID}-{Environment}-{Topic.Replace(".", "-")}";

            var subscriptionConfiguration = new NatsStreamingSubscriptionConfigurationBuilder()
                .WithSubscriptionName(topic)
                .Build();

            _natsStreamingConsumer = consumerFactory.Create(subscriptionConfiguration, clientId);
        }

        public void Connect()
        {
            _natsStreamingConsumer.Connect();
            _logger.LogInformation($"Consumer {Environment} connected on topic {Topic}");
        }

        public void Subscribe(Func<byte[], Task> handler)
        {
            _natsStreamingConsumer.Subscribe(async (bytes, properties) =>
            {
                await handler(bytes);
                _logger.LogInformation($"Receive message {Environment} - {Topic}");
            });
        }

        public void Dispose()
        {
            _natsStreamingConsumer?.Dispose();
        }
    }
}
