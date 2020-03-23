using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Processing.FeedProxyService.Configs;
using Processing.FeedProxyService.FeedProxies;
using Processing.FeedProxyService.Tests.Models;
using Processing.FeedProxyService.Tests.Utils.Builders;
using Xunit;

namespace Processing.FeedProxyService.Tests.UnitTests.FeedProxiesRunnerTests
{
    public class WhenFeedProxiesRunnerExecuteAsync
    {
        [Theory]
        [Trait(TestTraits.Category, TestCategories.Unit)]
        [Trait(TestTraits.Class, nameof(FeedProxiesRunner))]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(15)]
        public async Task ExecuteAsync_AllRunned(int topicCount)
        {
            var faker = new Faker();

            var topics = new TopicsConfig(faker.Random.WordsArray(topicCount));

            var runner = Create.FeedProxiesRunner().SetTopics(topics).Get();

            await runner.ExecuteAsyncWrap();

            runner.FeedProxies.Count.Should().Be(topicCount);
        }
    }
}
