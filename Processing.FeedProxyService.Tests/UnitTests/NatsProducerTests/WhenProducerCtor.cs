using System;
using Center.Processing.Framework.NatsStreaming.Interfaces;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using Processing.FeedProxyService.Model.Producer.NatsImplementation;
using Processing.FeedProxyService.Tests.Models;
using Xunit;

namespace Processing.FeedProxyService.Tests.UnitTests.NatsProducerTests
{
    public class WhenProducerCtor
    {
        [Fact]
        [Trait(TestTraits.Category, TestCategories.Unit)]
        [Trait(TestTraits.Class, nameof(NatsProducer))]
        public void Ctor_SetSameTierAndTopic()
        {
            var Environment = "Test";
            var topic = "Test.Topic";
            var factoryMock = new Mock<INatsStreamingProducerFactory>();
            NatsProducer natsProducer = null;

            Action act = () => { natsProducer = new NatsProducer(topic, Environment, factoryMock.Object); };

            act.Should().NotThrow();

            natsProducer.Should().NotBeNull();
            using (new AssertionScope())
            {
                natsProducer.Environment.Should().Be(Environment);
                natsProducer.Topic.Should().Be(topic);
            }
        }

        [Fact]
        [Trait(TestTraits.Category, TestCategories.Unit)]
        [Trait(TestTraits.Class, nameof(NatsProducer))]
        public void CtorTopicIsEmply_GetArgumentException()
        {
            var Environment = "Test";
            var factoryMock = new Mock<INatsStreamingProducerFactory>();
            NatsProducer natsProducer = null;

            Action act = () => { natsProducer = new NatsProducer("", Environment, factoryMock.Object); };

            act.Should().Throw<ArgumentNullException>("Topic(subscription) must me not empty.");
        }
    }
}
