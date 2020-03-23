using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Processing.FeedProxyService.Configs;

namespace Processing.FeedProxyService.FeedProxies
{
    public interface IFeedProxy : IDisposable
    {
        bool Run(string topic, ConsumerConfig consumeConfiguration, IEnumerable<ProducerConfig> produceConfigurations);
    }
}
