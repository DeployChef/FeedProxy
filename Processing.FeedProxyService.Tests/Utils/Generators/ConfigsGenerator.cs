using Bogus;
using Processing.FeedProxyService.Configs;
using System.Collections.Generic;

namespace Processing.FeedProxyService.Tests.Utils.Generators
{
    public class ConfigsGenerator
    {
        public static IEnumerable<ProducerConfig> GenerateProducerConfigs(int count)
        {
            var generator = new Faker<ProducerConfig>();
            return generator.Generate(count);
        }
    }
}
