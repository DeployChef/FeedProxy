using System;
using System.Threading.Tasks;
using Center.Processing.Framework.NatsStreaming.Interfaces;
using FluentAssertions;
using Moq;
using Processing.FeedProxyService.Model.Consumer.NatsImplementation;
using Processing.FeedProxyService.Tests.Models;
using Processing.FeedProxyService.Tests.Utils.Builders;
using Xunit;

namespace Processing.FeedProxyService.Tests.UnitTests.NatsConsumerTests
{
    public class WhenConsumerSubscribe
    {
        [Fact]
        [Trait(TestTraits.Category, TestCategories.Unit)]
        [Trait(TestTraits.Class, nameof(NatsConsumer))]
        public void Subscribe_Error()
        {
            var consumerMock = new Mock<INatsStreamingConsumer>();
            consumerMock.Setup(c => c.Subscribe(It.IsAny<Func<byte[], INatsStreamingSignalProperties, Task>>())).Throws<Exception>();

            var factory = Create.NatsConsumerFactory().SetupCreate(consumerMock.Object).Get();
            var natsConsumer = new NatsConsumer("topic", "Environment", factory);

            Action act = () => { natsConsumer.Subscribe(bytes => Task.CompletedTask); };

            act.Should().Throw<Exception>();
            consumerMock.Verify(c => c.Subscribe(It.IsAny<Func<byte[], INatsStreamingSignalProperties, Task>>()), Times.Once);
        }

        [Theory]
        [Trait(TestTraits.Category, TestCategories.Unit)]
        [Trait(TestTraits.Class, nameof(NatsConsumer))]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(4)]
        [InlineData(8)]
        public void Subscribe_HandleAllMessages(int messageCount)
        {
            var consumerMock = new Mock<INatsStreamingConsumer>();
            consumerMock.Setup(c => c.Subscribe(It.IsAny<Func<byte[], INatsStreamingSignalProperties, Task>>()))
                .Callback((Func<byte[], INatsStreamingSignalProperties, Task> handler) =>
                {
                    for (var i = 0; i < messageCount; i++)
                    {
                        handler(new byte[0], new Mock<INatsStreamingSignalProperties>().Object).Wait();
                    }
                });

            var factory = Create.NatsConsumerFactory().SetupCreate(consumerMock.Object).Get();
            var natsConsumer = new NatsConsumer("topic", "Environment", factory);

            var receiveMessagesCount = 0;

            natsConsumer.Subscribe(bytes =>
            {
                receiveMessagesCount++;
                return Task.CompletedTask;
            });

            receiveMessagesCount.Should().Be(messageCount);
        }
    }
}
