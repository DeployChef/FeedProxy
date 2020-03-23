using System;
using FluentAssertions;
using Processing.FeedProxyService.Configs;
using Processing.FeedProxyService.Model.Consumer.NatsImplementation;
using Processing.FeedProxyService.Tests.Models;
using Xunit;

namespace Processing.FeedProxyService.Tests.UnitTests.NatsConsumerFactoryTests
{
    public class WhenNatsConsumerFactoryCreate
    {
        [Fact]
        [Trait(TestTraits.Category, TestCategories.Unit)]
        [Trait(TestTraits.Class, nameof(NatsConsumerFactory))]
        public void Create_Success()
        {
            var factory = new NatsConsumerFactory();

            var consumer = factory.Create("topic", new ConsumerConfig());

            consumer.Should().NotBeNull();
        }

        [Fact]
        [Trait(TestTraits.Category, TestCategories.Unit)]
        [Trait(TestTraits.Class, nameof(NatsConsumerFactory))]
        public void CreateWithEmptyTopic_GetArgumentException()
        {
            var factory = new NatsConsumerFactory();

            Action act = () => { factory.Create("", new ConsumerConfig()); };

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        [Trait(TestTraits.Category, TestCategories.Unit)]
        [Trait(TestTraits.Class, nameof(NatsConsumerFactory))]
        public void CreateWithNullConfig_GetArgumentException()
        {
            var factory = new NatsConsumerFactory();

            Action act = () => { factory.Create("topic", null); };

            act.Should().Throw<ArgumentNullException>();
        }
    }
}
