using Center.Processing.Framework.NatsStreaming;
using Processing.FeedProxyService.Configs;
using Processing.FeedProxyService.Model.Consumer.Interfaces;
using System;

namespace Processing.FeedProxyService.Model.Consumer.NatsImplementation
{
    public class NatsConsumerFactory : IConsumerFactory
    {
        public IConsumer Create(string topic, ConsumerConfig configuration)
        {
            if (string.IsNullOrWhiteSpace(topic))
                throw new ArgumentNullException(nameof(topic), "Topic must be not null or empty");

            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            var connectionConfiguration = new NatsStreamingConnectionConfigurationBuilder()
                .WithClusterId(configuration.ClusterId)
                .WithNatsUrl(configuration.Urls)
                .Build();

            var factory = new NatsStreamingConsumerFactory(connectionConfiguration);

            return new NatsConsumer(topic, configuration.Environment, factory);
        }
    }
}
