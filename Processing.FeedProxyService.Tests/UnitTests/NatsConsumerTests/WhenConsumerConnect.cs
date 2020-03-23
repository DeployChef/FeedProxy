using System;
using Center.Processing.Framework.NatsStreaming.Interfaces;
using FluentAssertions;
using Moq;
using Processing.FeedProxyService.Model.Consumer.Interfaces;
using Processing.FeedProxyService.Model.Consumer.NatsImplementation;
using Processing.FeedProxyService.Tests.Models;
using Processing.FeedProxyService.Tests.Utils.Builders;
using Xunit;

namespace Processing.FeedProxyService.Tests.UnitTests.NatsConsumerTests
{
    public class WhenConsumerConnect
    {
        [Fact]
        [Trait(TestTraits.Category, TestCategories.Unit)]
        [Trait(TestTraits.Class, nameof(NatsConsumer))]
        public void Connect_Success()
        {
            var consumerMock = new Mock<INatsStreamingConsumer>();
            var factory = Create.NatsConsumerFactory().SetupCreate(consumerMock.Object).Get();
            IConsumer natsConsumer = new NatsConsumer("topic", "Environment", factory);

            Action act = () => { natsConsumer.Connect(); };

            act.Should().NotThrow();
            consumerMock.Verify(c => c.Connect(), Times.Once);
        }

        [Fact]
        [Trait(TestTraits.Category, TestCategories.Unit)]
        [Trait(TestTraits.Class, nameof(NatsConsumer))]
        public void Connect_Error()
        {
            var consumerMock = new Mock<INatsStreamingConsumer>();
            consumerMock.Setup(c => c.Connect()).Throws<Exception>();

            var factory = Create.NatsConsumerFactory().SetupCreate(consumerMock.Object).Get();
            IConsumer natsConsumer = new NatsConsumer("topic", "Environment", factory);

            Action act = () => { natsConsumer.Connect(); };

            act.Should().Throw<Exception>();
            consumerMock.Verify(c => c.Connect(), Times.Once);
        }
    }
}
