using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Processing.FeedProxyService.Configs;
using Processing.FeedProxyService.FeedProxies;
using Processing.FeedProxyService.Model.Consumer.Interfaces;
using Processing.FeedProxyService.Model.Producer.Interfaces;
using Processing.FeedProxyService.Tests.Models;
using Processing.FeedProxyService.Tests.Utils.Builders;
using Processing.FeedProxyService.Tests.Utils.Generators;
using Xunit;

namespace Processing.FeedProxyService.Tests.UnitTests.FeedProxyTests
{
    public class WhenFeedProxyRun
    {
        [Theory]
        [Trait(TestTraits.Category, TestCategories.Unit)]
        [Trait(TestTraits.Class, nameof(FeedProxy))]
        [MemberData(nameof(ParametersGenerator.GetProducersCommon), MemberType = typeof(ParametersGenerator))]
        public void RunCommonSituations_ReturnTrue(List<Mock<IProducer>> producerMocks)
        {
            var producerConfigs = ConfigsGenerator.GenerateProducerConfigs(producerMocks.Count);

            var consumerMock = new Mock<IConsumer>();

            var consumerFactory = Create.ConsumerFactory().SetupCreate(consumerMock.Object).Get();
            var producerFactory = Create.ProducerFactory().SetupCreate(producerMocks.Select(c => c.Object)).Get();

            var feedProxy = new FeedProxy(consumerFactory, producerFactory);

            var isStarted = feedProxy.Run("test", new ConsumerConfig(), producerConfigs);

            isStarted.Should().BeTrue();

            consumerMock.Verify(c => c.Connect(), Times.Once);
            producerMocks.ForEach(q => q.Verify(c => c.Connect(), Times.Once));

            consumerMock.Verify(c => c.Subscribe(It.IsAny<Func<byte[], Task>>()), Times.Once);
        }

        [Fact]
        [Trait(TestTraits.Category, TestCategories.Unit)]
        [Trait(TestTraits.Class, nameof(FeedProxy))]
        public void RunWhenConsumerNotConnected_ReturnFalse()
        {
            var consumerMock = new Mock<IConsumer>();
            consumerMock.Setup(c => c.Connect()).Throws<Exception>();

            var consumerFactory = Create.ConsumerFactory().SetupCreate(consumerMock.Object).Get();

            var producerMock = new Mock<IProducer>();
            var producerFactory = Create.ProducerFactory().SetupCreate(producerMock.Object).Get();

            var feedProxy = new FeedProxy(consumerFactory, producerFactory);

            var isStarted = feedProxy.Run("test", new ConsumerConfig(), new List<ProducerConfig>());

            isStarted.Should().BeFalse("Consumer not connected");
            consumerMock.Verify(c => c.Connect(), Times.Once);
            producerMock.Verify(c => c.Connect(), Times.Never);
        }

        [Theory]
        [Trait(TestTraits.Category, TestCategories.Unit)]
        [Trait(TestTraits.Class, nameof(FeedProxy))]
        [MemberData(nameof(ParametersGenerator.GetProducersCommon), MemberType = typeof(ParametersGenerator))]
        public void RunWhenAllProducersNotConnected_ReturnFalse(List<Mock<IProducer>> producerMocks)
        {
            var producerConfigs = ConfigsGenerator.GenerateProducerConfigs(producerMocks.Count);

            var consumerMock = new Mock<IConsumer>();
            var consumerFactory = Create.ConsumerFactory().SetupCreate(consumerMock.Object).Get();

            var producerFactory = Create.ProducerFactory().SetupCreate(producerMocks.Select(c => c.Object)).Get();
            producerMocks.ForEach(q => q.Setup(c => c.Connect()).Throws<Exception>());

            var feedProxy = new FeedProxy(consumerFactory, producerFactory);

            var isStarted = feedProxy.Run("test", new ConsumerConfig(), producerConfigs);

            isStarted.Should().BeFalse("Producers not connected");
            producerMocks.ForEach(q => q.Verify(c => c.Connect(), Times.Once));
            consumerMock.Verify(c => c.Subscribe(It.IsAny<Func<byte[], Task>>()), Times.Never);
        }

        [Theory]
        [Trait(TestTraits.Category, TestCategories.Unit)]
        [Trait(TestTraits.Class, nameof(FeedProxy))]
        [MemberData(nameof(ParametersGenerator.ManyProducersNotConnected), MemberType = typeof(ParametersGenerator))]
        public void RunWhenManyProducersNotConnected_ReturnTrue(List<Mock<IProducer>> producerMocks, int normalProducersCount)
        {
            var producerConfigs = ConfigsGenerator.GenerateProducerConfigs(producerMocks.Count);

            var consumerMock = new Mock<IConsumer>();
            var consumerFactory = Create.ConsumerFactory().SetupCreate(consumerMock.Object).Get();

            var producerFactory = Create.ProducerFactory().SetupCreate(producerMocks.Select(c => c.Object)).Get();

            var feedProxy = new FeedProxy(consumerFactory, producerFactory);

            var isStarted = feedProxy.Run("test", new ConsumerConfig(), producerConfigs);

            isStarted.Should().BeTrue();
            producerMocks.ForEach(q => q.Verify(c => c.Connect(), Times.Once));
            feedProxy.ConnectedProducersCount.Should().Be(normalProducersCount);
            consumerMock.Verify(c => c.Subscribe(It.IsAny<Func<byte[], Task>>()), Times.Once);
        }
    }
}
