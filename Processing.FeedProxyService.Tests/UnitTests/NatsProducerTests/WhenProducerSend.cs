using System;
using System.Threading.Tasks;
using Center.Processing.Framework.NatsStreaming.Interfaces;
using FluentAssertions;
using Moq;
using Processing.FeedProxyService.Model.Producer.NatsImplementation;
using Processing.FeedProxyService.Tests.Models;
using Processing.FeedProxyService.Tests.Utils.Builders;
using Xunit;

namespace Processing.FeedProxyService.Tests.UnitTests.NatsProducerTests
{
    public class WhenProducerSend
    {
        [Fact]
        [Trait(TestTraits.Category, TestCategories.Unit)]
        [Trait(TestTraits.Class, nameof(NatsProducer))]
        public void Send_Success()
        {
            var producerMock = new Mock<INatsStreamingProducer>();

            var factory = Create.NatsProducerFactory().SetupCreate(producerMock.Object).Get();
            var natsConsumer = new NatsProducer("topic", "Environment", factory);

            Func<Task> act = async () => { await natsConsumer.SendAsync(new byte[0]); };

            act.Should().NotThrow();

            producerMock.Verify(c => c.SendAsync(It.IsAny<byte[]>()), Times.Once);
        }

        [Fact]
        [Trait(TestTraits.Category, TestCategories.Unit)]
        [Trait(TestTraits.Class, nameof(NatsProducer))]
        public void Send_Error()
        {
            var producerMock = new Mock<INatsStreamingProducer>();
            producerMock.Setup(c => c.SendAsync(It.IsAny<byte[]>())).Throws<Exception>();

            var factory = Create.NatsProducerFactory().SetupCreate(producerMock.Object).Get();
            var natsConsumer = new NatsProducer("topic", "Environment", factory);

            Func<Task> act = async () => { await natsConsumer.SendAsync(new byte[0]); };

            act.Should().Throw<Exception>();

            producerMock.Verify(c => c.SendAsync(It.IsAny<byte[]>()), Times.Once);
        }
    }
}
