using Center.Processing.Framework.NatsStreaming.Interfaces;
using Center.Processing.Framework.NatsStreaming.Serializers;
using Moq;

namespace Processing.FeedProxyService.Tests.Utils.Builders
{
    public class NatsProducerFactoryBuilder
    {
        private readonly Mock<INatsStreamingProducerFactory> _consumerFactoryMock;

        public NatsProducerFactoryBuilder()
        {
            _consumerFactoryMock = new Mock<INatsStreamingProducerFactory>();
        }

        public NatsProducerFactoryBuilder SetupCreate(INatsStreamingProducer consumer)
        {
            _consumerFactoryMock.Setup(c => c.Create(It.IsAny<INatsStreamingSubscriptionConfiguration>(), It.IsAny<string>(), It.IsAny<ISerializer>()))
                .Returns(consumer);
            return this;
        }

        public INatsStreamingProducerFactory Get()
        {
            return _consumerFactoryMock.Object;
        }
    }
}
