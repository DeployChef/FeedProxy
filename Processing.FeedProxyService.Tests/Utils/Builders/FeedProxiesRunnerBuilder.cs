using Processing.FeedProxyService.Tests.Models;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using Processing.FeedProxyService.Configs;
using Processing.FeedProxyService.FeedProxies;

namespace Processing.FeedProxyService.Tests.Utils.Builders
{
    public class FeedProxiesRunnerBuilder
    {
        private TopicsConfig _topicsConfig = new TopicsConfig() { "topic1", "topic2", "topic3" };
        private ConsumerConfig _consumerConfig = new ConsumerConfig() { Environment = "TestTier", ClusterId = "TestCluster", Urls = "TestUrls" };
        private ProducerConfigs _producerConfigs = new ProducerConfigs() {new ProducerConfig{ Environment = "TestTierProducer1", ClusterId = "TestClusterProducer1", Urls = "TestUrlsProducer1" }};
        private readonly IServiceCollection _serviceCollection = new ServiceCollection();

        public FeedProxiesRunnerBuilder SetTopics(TopicsConfig topicsConfig)
        {
            _topicsConfig = topicsConfig;
            return this;
        }

        public FeedProxiesRunnerBuilder SetConsumerConfig(ConsumerConfig consumerConfig)
        {
            _consumerConfig = consumerConfig;
            return this;
        }

        public FeedProxiesRunnerBuilder SetProducerConfigs(ProducerConfigs producerConfigs)
        {
            _producerConfigs = producerConfigs;
            return this;
        }

        public FeedProxiesRunnerTestDecorator Get()
        {
            var feedProxyMock = new Mock<IFeedProxy>();
            feedProxyMock.Setup(c =>
                    c.Run(It.IsAny<string>(), It.IsAny<ConsumerConfig>(), It.IsAny<IEnumerable<ProducerConfig>>()))
                .Returns(true);
            _serviceCollection.AddTransient(serviceProvider => feedProxyMock.Object);
            var provider = _serviceCollection.BuildServiceProvider();
            var topicsOptions = new Mock<IOptions<TopicsConfig>>();
            topicsOptions.SetupGet(c => c.Value).Returns(_topicsConfig);
            var consumerConfigOptions = new Mock<IOptions<ConsumerConfig>>();
            consumerConfigOptions.SetupGet(c => c.Value).Returns(_consumerConfig);
            var producerConfigsOptions = new Mock<IOptions<ProducerConfigs>>();
            producerConfigsOptions.SetupGet(c => c.Value).Returns(_producerConfigs);
            
            var runner = new FeedProxiesRunnerTestDecorator(provider, topicsOptions.Object, 
                consumerConfigOptions.Object, producerConfigsOptions.Object);

            return runner;
        }
    }
}
