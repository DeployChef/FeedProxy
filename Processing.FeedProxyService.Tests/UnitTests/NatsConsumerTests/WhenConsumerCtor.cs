using System;
using Center.Processing.Framework.NatsStreaming.Interfaces;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using Processing.FeedProxyService.Model.Consumer.Interfaces;
using Processing.FeedProxyService.Model.Consumer.NatsImplementation;
using Processing.FeedProxyService.Tests.Models;
using Xunit;

namespace Processing.FeedProxyService.Tests.UnitTests.NatsConsumerTests
{
    public class WhenConsumerCtor
    {
        [Fact]
        [Trait(TestTraits.Category, TestCategories.Unit)]
        [Trait(TestTraits.Class, nameof(NatsConsumer))]
        public void Ctor_SetSameTierAndTopic()
        {
            var Environment = "Test";
            var topic = "Test.Topic";
            var factoryMock = new Mock<INatsStreamingConsumerFactory>();
            IConsumer natsConsumer = null;

            Action act = () => { natsConsumer = new NatsConsumer(topic, Environment, factoryMock.Object); };

            act.Should().NotThrow();

            natsConsumer.Should().NotBeNull();
            using (new AssertionScope())
            {
                natsConsumer.Environment.Should().Be(Environment);
                natsConsumer.Topic.Should().Be(topic);
            }
        }

        [Fact]
        [Trait(TestTraits.Category, TestCategories.Unit)]
        [Trait(TestTraits.Class, nameof(NatsConsumer))]
        public void CtorTopicIsEmply_GetArgumentException()
        {
            var Environment = "Test";
            var factoryMock = new Mock<INatsStreamingConsumerFactory>();
            IConsumer natsConsumer = null;

            Action act = () => { natsConsumer = new NatsConsumer("", Environment, factoryMock.Object); };

            act.Should().Throw<ArgumentNullException>("Topic(subscription) must me not empty.");
        }
    }
}
