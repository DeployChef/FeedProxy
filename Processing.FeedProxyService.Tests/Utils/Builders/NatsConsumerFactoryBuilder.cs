using Center.Processing.Framework.NatsStreaming.Interfaces;
using Center.Processing.Framework.NatsStreaming.Serializers;
using Moq;

namespace Processing.FeedProxyService.Tests.Utils.Builders
{
    public class NatsConsumerFactoryBuilder
    {
        private readonly Mock<INatsStreamingConsumerFactory> _consumerFactoryMock;

        public NatsConsumerFactoryBuilder()
        {
            _consumerFactoryMock = new Mock<INatsStreamingConsumerFactory>();
        }

        public NatsConsumerFactoryBuilder SetupCreate(INatsStreamingConsumer consumer)
        {
            _consumerFactoryMock.Setup(c => c.Create(It.IsAny<INatsStreamingSubscriptionConfiguration>(), It.IsAny<string>(), It.IsAny<IDeserializer>()))
                .Returns(consumer);
            return this;
        }

        public INatsStreamingConsumerFactory Get()
        {
            return _consumerFactoryMock.Object;
        }
    }
}
