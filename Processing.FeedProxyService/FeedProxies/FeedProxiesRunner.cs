using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Processing.FeedProxyService.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Processing.FeedProxyService.FeedProxies
{
    public class FeedProxiesRunner : BackgroundService
    {
        private readonly IServiceProvider _provider;
        private readonly ILogger<FeedProxiesRunner> _logger;

        protected readonly List<IFeedProxy> _feedProxies = new List<IFeedProxy>();

        protected readonly ConsumerConfig _consumerConfig;
        protected readonly ProducerConfigs _producerConfig;
        protected readonly string[] _topics;

        public FeedProxiesRunner(
            IServiceProvider provider,
            IOptions<TopicsConfig> topicsOptions,
            IOptions<ConsumerConfig> consumerOptions,
            IOptions<ProducerConfigs> producerOptions,
            ILogger<FeedProxiesRunner> logger = null)
        {
            _provider = provider;
            _logger = logger ?? new NullLogger<FeedProxiesRunner>();

            if (topicsOptions?.Value == null || !topicsOptions.Value.Any())
            {
                _logger.LogWarning($"TopicsConfig is empty!");
                throw new ArgumentNullException(nameof(topicsOptions), "TopicsConfig is empty");
            }
            else
            {
                _topics = topicsOptions.Value.ToArray();
            }

            if (consumerOptions?.Value == null)
            {
                _consumerConfig = new ConsumerConfig();
                _logger.LogWarning($"Consumer configuration is null. The default consumer configuration will be used!");
            }
            else
            {
                _consumerConfig = consumerOptions.Value;
            }

            if (producerOptions?.Value == null || !producerOptions.Value.Any())
            {
                _logger.LogWarning($"ProducerConfigs is empty!");
                throw new ArgumentNullException(nameof(producerOptions), "ProducerConfigs is empty");
            }
            else
            {
                _producerConfig = producerOptions.Value;
            }
        }

        protected override Task ExecuteAsync(CancellationToken token)
        {
            _logger.LogInformation($"Starting feed proxies...");

            foreach (var topic in _topics)
            {
                try
                {
                    var feedProxy = _provider.GetService<IFeedProxy>();
                    var isStarted = feedProxy.Run(topic, _consumerConfig, _producerConfig);
                    if (!isStarted)
                    {
                        _logger.LogWarning($"Cannot start on {topic}. Service stopped!");
                        return Task.CompletedTask;
                    }

                    _feedProxies.Add(feedProxy);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"Exception while start feedProxy {topic}.");
                }
            }

            return Task.CompletedTask;
        }
    }
}
