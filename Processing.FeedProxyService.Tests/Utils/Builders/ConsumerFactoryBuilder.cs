using Moq;
using Processing.FeedProxyService.Configs;
using Processing.FeedProxyService.Model.Consumer.Interfaces;

namespace Processing.FeedProxyService.Tests.Utils.Builders
{
    public class ConsumerFactoryBuilder
    {
        private readonly Mock<IConsumerFactory> _consumerFactoryMock;

        public ConsumerFactoryBuilder()
        {
            _consumerFactoryMock = new Mock<IConsumerFactory>();
        }

        public ConsumerFactoryBuilder SetupCreate(IConsumer consumer)
        {
            _consumerFactoryMock.SetupSequence(c => c.Create(It.IsAny<string>(), It.IsAny<ConsumerConfig>()))
                .Returns(consumer);
            return this;
        }

        public IConsumerFactory Get()
        {
            return _consumerFactoryMock.Object;
        }
    }
}
