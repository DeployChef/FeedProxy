using Processing.FeedProxyService.FeedProxies;

namespace Processing.FeedProxyService.Tests.Utils.Builders
{
    public static class Create
    {
        public static ConsumerBuilder Consumer() => new ConsumerBuilder("Fake");

        public static ProducerFactoryBuilder ProducerFactory() => new ProducerFactoryBuilder();

        public static ConsumerFactoryBuilder ConsumerFactory() => new ConsumerFactoryBuilder();

        public static NatsConsumerFactoryBuilder NatsConsumerFactory() => new NatsConsumerFactoryBuilder();

        public static NatsProducerFactoryBuilder NatsProducerFactory() => new NatsProducerFactoryBuilder();

        public static FeedProxiesRunnerBuilder FeedProxiesRunner() => new FeedProxiesRunnerBuilder();
    }
}
