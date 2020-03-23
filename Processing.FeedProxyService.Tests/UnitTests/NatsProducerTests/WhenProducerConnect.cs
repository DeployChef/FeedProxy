using System;
using Center.Processing.Framework.NatsStreaming.Interfaces;
using FluentAssertions;
using Moq;
using Processing.FeedProxyService.Model.Producer.NatsImplementation;
using Processing.FeedProxyService.Tests.Models;
using Processing.FeedProxyService.Tests.Utils.Builders;
using Xunit;

namespace Processing.FeedProxyService.Tests.UnitTests.NatsProducerTests
{
    public class WhenProducerConnect
    {
        [Fact]
        [Trait(TestTraits.Category, TestCategories.Unit)]
        [Trait(TestTraits.Class, nameof(NatsProducer))]
        public void Connect_Success()
        {
            var producerMock = new Mock<INatsStreamingProducer>();
            var factory = Create.NatsProducerFactory().SetupCreate(producerMock.Object).Get();
            var natsConsumer = new NatsProducer("topic", "Environment", factory);

            Action act = () => { natsConsumer.Connect(); };

            act.Should().NotThrow();
            producerMock.Verify(c => c.Connect(), Times.Once);
        }

        [Fact]
        [Trait(TestTraits.Category, TestCategories.Unit)]
        [Trait(TestTraits.Class, nameof(NatsProducer))]
        public void Connect_Error()
        {
            var producerMock = new Mock<INatsStreamingProducer>();
            producerMock.Setup(c => c.Connect()).Throws<Exception>();

            var factory = Create.NatsProducerFactory().SetupCreate(producerMock.Object).Get();
            var natsConsumer = new NatsProducer("topic", "Environment", factory);

            Action act = () => { natsConsumer.Connect(); };

            act.Should().Throw<Exception>();
            producerMock.Verify(c => c.Connect(), Times.Once);
        }
    }
}
