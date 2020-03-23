using System;
using FluentAssertions;
using Processing.FeedProxyService.FeedProxies;
using Processing.FeedProxyService.Model.Consumer.NatsImplementation;
using Processing.FeedProxyService.Model.Producer.NatsImplementation;
using Processing.FeedProxyService.Tests.Models;
using Xunit;

namespace Processing.FeedProxyService.Tests.UnitTests.FeedProxyTests
{
    public class WhenFeedProxyCtor
    {
        [Fact]
        [Trait(TestTraits.Category, TestCategories.Unit)]
        [Trait(TestTraits.Class, nameof(FeedProxy))]
        public void Ctor_Success()
        {
            Action act = () => new FeedProxy(new NatsConsumerFactory(), new NatsProducerFactory());

            act.Should().NotThrow();
        }

        [Fact]
        [Trait(TestTraits.Category, TestCategories.Unit)]
        [Trait(TestTraits.Class, nameof(FeedProxy))]
        public void CtorNullConsumerFactory_GetConsumerFactoryArgumentNullException()
        {
            Action act = () => new FeedProxy(null, new NatsProducerFactory());

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        [Trait(TestTraits.Category, TestCategories.Unit)]
        [Trait(TestTraits.Class, nameof(FeedProxy))]
        public void CtorNullProducerFactory_GetProducerFactoryArgumentNullException()
        {
            Action act = () => new FeedProxy(new NatsConsumerFactory(), null);

            act.Should().Throw<ArgumentNullException>();
        }
    }
}
