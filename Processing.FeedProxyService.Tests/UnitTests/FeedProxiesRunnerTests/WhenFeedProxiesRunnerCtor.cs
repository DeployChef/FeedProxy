using System;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Processing.FeedProxyService.Configs;
using Processing.FeedProxyService.FeedProxies;
using Processing.FeedProxyService.Tests.Models;
using Xunit;

namespace Processing.FeedProxyService.Tests.UnitTests.FeedProxiesRunnerTests
{
    public class WhenFeedProxiesRunnerCtor
    {
        [Fact]
        [Trait(TestTraits.Category, TestCategories.Unit)]
        [Trait(TestTraits.Class, nameof(FeedProxiesRunner))]
        public void CtorCommon_SameConfigsSuccess()
        {
            var topicsConfig = new TopicsConfig(){"topic1","topic2","topic3"};
            var consumerConfig = new ConsumerConfig() {Environment = "TestTier", ClusterId = "TestCluster", Urls = "TestUrls"};
            var producerConfigs = new ProducerConfigs()
            {
                {new ProducerConfig{ Environment = "TestTierProducer1", ClusterId = "TestClusterProducer1", Urls = "TestUrlsProducer1" }},
                {new ProducerConfig{ Environment = "TestTierProducer2", ClusterId = "TestClusterProducer2", Urls = "TestUrlsProducer2" }}
            };

            var provider = new Mock<IServiceProvider>();
            var topicsOptions = new Mock<IOptions<TopicsConfig>>();
            topicsOptions.SetupGet(c => c.Value).Returns(topicsConfig);
            var consumerConfigOptions = new Mock<IOptions<ConsumerConfig>>();
            consumerConfigOptions.SetupGet(c => c.Value).Returns(consumerConfig);
            var producerConfigsOptions = new Mock<IOptions<ProducerConfigs>>();
            producerConfigsOptions.SetupGet(c => c.Value).Returns(producerConfigs);

            var runner = new FeedProxiesRunnerTestDecorator(provider.Object, topicsOptions.Object,
                consumerConfigOptions.Object, producerConfigsOptions.Object);

            runner.Topics.Should().BeEquivalentTo(topicsConfig);
            runner.ConsumerConfig.Should().BeEquivalentTo(consumerConfig);
            runner.ProducerConfigs.Should().BeEquivalentTo(producerConfigs);
        }

        [Fact]
        [Trait(TestTraits.Category, TestCategories.Unit)]
        [Trait(TestTraits.Class, nameof(FeedProxiesRunner))]
        public void CtorNullTopicConfig_ArgumentNullException()
        {
            var consumerConfig = new ConsumerConfig();
            var producerConfigs = new ProducerConfigs()
            {
                {new ProducerConfig{ Environment = "TestTierProducer1", ClusterId = "TestClusterProducer1", Urls = "TestUrlsProducer1" }},
                {new ProducerConfig{ Environment = "TestTierProducer2", ClusterId = "TestClusterProducer2", Urls = "TestUrlsProducer2" }}
            };

            var provider = new Mock<IServiceProvider>();
            var consumerConfigOptions = new Mock<IOptions<ConsumerConfig>>();
            consumerConfigOptions.SetupGet(c => c.Value).Returns(consumerConfig);
            var producerConfigsOptions = new Mock<IOptions<ProducerConfigs>>();
            producerConfigsOptions.SetupGet(c => c.Value).Returns(producerConfigs);

            Action act = () => new FeedProxiesRunnerTestDecorator(provider.Object, null,
                    consumerConfigOptions.Object, producerConfigsOptions.Object);
            
            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("topicsOptions");
        }

        [Fact]
        [Trait(TestTraits.Category, TestCategories.Unit)]
        [Trait(TestTraits.Class, nameof(FeedProxiesRunner))]
        public void CtorEmptyTopicConfig_ArgumentNullException()
        {
            var topicsConfig = new TopicsConfig();
            var consumerConfig = new ConsumerConfig();
            var producerConfigs = new ProducerConfigs()
            {
                {new ProducerConfig{ Environment = "TestTierProducer1", ClusterId = "TestClusterProducer1", Urls = "TestUrlsProducer1" }},
                {new ProducerConfig{ Environment = "TestTierProducer2", ClusterId = "TestClusterProducer2", Urls = "TestUrlsProducer2" }}
            };

            var provider = new Mock<IServiceProvider>();
            var topicsOptions = new Mock<IOptions<TopicsConfig>>();
            topicsOptions.SetupGet(c => c.Value).Returns(topicsConfig);
            var consumerConfigOptions = new Mock<IOptions<ConsumerConfig>>();
            consumerConfigOptions.SetupGet(c => c.Value).Returns(consumerConfig);
            var producerConfigsOptions = new Mock<IOptions<ProducerConfigs>>();
            producerConfigsOptions.SetupGet(c => c.Value).Returns(producerConfigs);

            Action act = () => new FeedProxiesRunnerTestDecorator(provider.Object, topicsOptions.Object,
                consumerConfigOptions.Object, producerConfigsOptions.Object);

            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("topicsOptions");
        }

        [Fact]
        [Trait(TestTraits.Category, TestCategories.Unit)]
        [Trait(TestTraits.Class, nameof(FeedProxiesRunner))]
        public void CtorNullProducerConfig_ArgumentNullException()
        {
            var topicsConfig = new TopicsConfig() { "topic1", "topic2", "topic3" };
            var consumerConfig = new ConsumerConfig();

            var provider = new Mock<IServiceProvider>();
            var topicsOptions = new Mock<IOptions<TopicsConfig>>();
            topicsOptions.SetupGet(c => c.Value).Returns(topicsConfig);
            var consumerConfigOptions = new Mock<IOptions<ConsumerConfig>>();
            consumerConfigOptions.SetupGet(c => c.Value).Returns(consumerConfig);

            Action act = () => new FeedProxiesRunnerTestDecorator(provider.Object, topicsOptions.Object,
                consumerConfigOptions.Object, null);

            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("producerOptions");
        }

        [Fact]
        [Trait(TestTraits.Category, TestCategories.Unit)]
        [Trait(TestTraits.Class, nameof(FeedProxiesRunner))]
        public void CtorEmptyProducerConfig_ArgumentNullException()
        {
            var topicsConfig = new TopicsConfig() { "topic1", "topic2", "topic3" };
            var consumerConfig = new ConsumerConfig();
            var producerConfigs = new ProducerConfigs();

            var provider = new Mock<IServiceProvider>();
            var topicsOptions = new Mock<IOptions<TopicsConfig>>();
            topicsOptions.SetupGet(c => c.Value).Returns(topicsConfig);
            var consumerConfigOptions = new Mock<IOptions<ConsumerConfig>>();
            consumerConfigOptions.SetupGet(c => c.Value).Returns(consumerConfig);
            var producerConfigsOptions = new Mock<IOptions<ProducerConfigs>>();
            producerConfigsOptions.SetupGet(c => c.Value).Returns(producerConfigs);

            Action act = () => new FeedProxiesRunnerTestDecorator(provider.Object, topicsOptions.Object,
                consumerConfigOptions.Object, producerConfigsOptions.Object);

            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("producerOptions");
        }
    }
}
