using Center.Processing.Framework.NatsStreaming;
using Processing.FeedProxyService.Configs;
using Processing.FeedProxyService.Model.Producer.Interfaces;
using System;

namespace Processing.FeedProxyService.Model.Producer.NatsImplementation
{
    public class NatsProducerFactory : IProducerFactory
    {
        public IProducer Create(string topic, ProducerConfig configuration)
        {
            if (string.IsNullOrWhiteSpace(topic))
                throw new ArgumentNullException(nameof(topic), "Topic must be not null or empty");

            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            var connectionConfiguration = new NatsStreamingConnectionConfigurationBuilder()
                .WithClusterId(configuration.ClusterId)
                .WithNatsUrl(configuration.Urls)
                .Build();

            var factory = new NatsStreamingProducerFactory(connectionConfiguration);

            return new NatsProducer(topic, configuration.Environment, factory);
        }
    }
}
