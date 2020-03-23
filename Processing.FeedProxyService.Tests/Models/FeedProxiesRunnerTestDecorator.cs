using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Processing.FeedProxyService.Configs;
using Processing.FeedProxyService.FeedProxies;

namespace Processing.FeedProxyService.Tests.Models
{
    public class FeedProxiesRunnerTestDecorator : FeedProxiesRunner
    {
        public ConsumerConfig ConsumerConfig => _consumerConfig;

        public ProducerConfigs ProducerConfigs => _producerConfig;

        public List<IFeedProxy> FeedProxies => _feedProxies;

        public string[] Topics => _topics;

        public async Task ExecuteAsyncWrap()
        {
            await ExecuteAsync(CancellationToken.None);
        }

        public FeedProxiesRunnerTestDecorator(IServiceProvider provider, IOptions<TopicsConfig> topicsOptions, IOptions<ConsumerConfig> consumerOptions, IOptions<ProducerConfigs> producerOptions, ILogger<FeedProxiesRunner> logger = null) : base(provider, topicsOptions, consumerOptions, producerOptions, logger)
        {
        }
    }
}
