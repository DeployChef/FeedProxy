using System;
using FluentAssertions;
using Processing.FeedProxyService.Configs;
using Processing.FeedProxyService.Model.Producer.NatsImplementation;
using Processing.FeedProxyService.Tests.Models;
using Xunit;

namespace Processing.FeedProxyService.Tests.UnitTests.NatsProducerFactoryTests
{
    public class WhenNatsProducerFactoryCreate
    {
        [Fact]
        [Trait(TestTraits.Category, TestCategories.Unit)]
        [Trait(TestTraits.Class, nameof(NatsProducerFactory))]
        public void Create_Success()
        {
            var factory = new NatsProducerFactory();

            var producer = factory.Create("topic", new ProducerConfig());

            producer.Should().NotBeNull();
        }

        [Fact]
        [Trait(TestTraits.Category, TestCategories.Unit)]
        [Trait(TestTraits.Class, nameof(NatsProducerFactory))]
        public void CreateWithEmptyTopic_GetArgumentException()
        {
            var factory = new NatsProducerFactory();

            Action act = () => { factory.Create("", new ProducerConfig()); };

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        [Trait(TestTraits.Category, TestCategories.Unit)]
        [Trait(TestTraits.Class, nameof(NatsProducerFactory))]
        public void CreateWithNullConfig_GetArgumentException()
        {
            var factory = new NatsProducerFactory();

            Action act = () => { factory.Create("topic", null); };

            act.Should().Throw<ArgumentNullException>();
        }
    }
}
