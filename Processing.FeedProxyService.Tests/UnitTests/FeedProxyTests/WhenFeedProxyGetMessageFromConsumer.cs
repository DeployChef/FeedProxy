using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Processing.FeedProxyService.Configs;
using Processing.FeedProxyService.FeedProxies;
using Processing.FeedProxyService.Model.Producer.Interfaces;
using Processing.FeedProxyService.Tests.Models;
using Processing.FeedProxyService.Tests.Utils.Builders;
using Processing.FeedProxyService.Tests.Utils.Generators;
using Xunit;

namespace Processing.FeedProxyService.Tests.UnitTests.FeedProxyTests
{
    public class WhenFeedProxyGetMessageFromConsumer
    {
        [Theory]
        [Trait(TestTraits.Category, TestCategories.Unit)]
        [Trait(TestTraits.Class, nameof(FeedProxy))]
        [MemberData(nameof(ParametersGenerator.GetMessageFromConsumerParameters), MemberType = typeof(ParametersGenerator))]
        public async Task GetMessageBySubscription_AllProducersSended(List<Mock<IProducer>> producerMocks, int messageCount)
        {
            var producerConfigs = ConfigsGenerator.GenerateProducerConfigs(producerMocks.Count);

            var consumerFake = Create.Consumer().SetEventCounts(messageCount).Get();

            var consumerFactory = Create.ConsumerFactory().SetupCreate(consumerFake).Get();
            var producerFactory = Create.ProducerFactory().SetupCreate(producerMocks.Select(c => c.Object)).Get();

            var feedProxy = new FeedProxy(consumerFactory, producerFactory);

            feedProxy.Run("test", new ConsumerConfig(), producerConfigs);

            while (!consumerFake.SendedAll)
            {
                await Task.Delay(10); //Subscription work
            }

            producerMocks.ForEach(q => q.Verify(c => c.Connect(), Times.Once));
            producerMocks.ForEach(q => q.Verify(c => c.SendAsync(It.IsAny<byte[]>()), Times.Exactly(messageCount)));
        }

        [Theory]
        [Trait(TestTraits.Category, TestCategories.Unit)]
        [Trait(TestTraits.Class, nameof(FeedProxy))]
        [MemberData(nameof(ParametersGenerator.GetMessageFromConsumerNotConnectedProducersParameters), MemberType = typeof(ParametersGenerator))]
        public async Task GetMessageBySubscriptionNotConnectedProducers_AllProducersSended(List<Mock<IProducer>> producerMocks, int messageCount)
        {
            var producerConfigs = ConfigsGenerator.GenerateProducerConfigs(producerMocks.Count);

            var consumerFake = Create.Consumer().SetEventCounts(messageCount).Get();

            var consumerFactory = Create.ConsumerFactory().SetupCreate(consumerFake).Get();
            var producerFactory = Create.ProducerFactory().SetupCreate(producerMocks.Select(c => c.Object)).Get();

            var feedProxy = new FeedProxy(consumerFactory, producerFactory);

            feedProxy.Run("test", new ConsumerConfig(), producerConfigs);

            while (!consumerFake.SendedAll)
            {
                await Task.Delay(10); //Subscription work
            }

            producerMocks.ForEach(q => q.Verify(c => c.Connect(), Times.Once));
            producerMocks.ForEach(q => q.Verify(c => c.SendAsync(It.IsAny<byte[]>()), Times.Exactly(messageCount)));
        }
    }
}
