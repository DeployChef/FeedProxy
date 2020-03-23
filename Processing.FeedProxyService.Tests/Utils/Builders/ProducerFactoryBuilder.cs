using Moq;
using Processing.FeedProxyService.Configs;
using Processing.FeedProxyService.Model.Producer.Interfaces;
using System.Collections.Generic;

namespace Processing.FeedProxyService.Tests.Utils.Builders
{
    public class ProducerFactoryBuilder
    {
        private readonly Mock<IProducerFactory> _producerFactoryMock;

        public ProducerFactoryBuilder()
        {
            _producerFactoryMock = new Mock<IProducerFactory>();
        }

        public ProducerFactoryBuilder SetupCreate(IProducer producer)
        {
            _producerFactoryMock.SetupSequence(c => c.Create(It.IsAny<string>(), It.IsAny<ProducerConfig>()))
                                .Returns(producer);
            return this;
        }

        public ProducerFactoryBuilder SetupCreate(IEnumerable<IProducer> producers)
        {
            var sequence = _producerFactoryMock.SetupSequence(c => c.Create(It.IsAny<string>(), It.IsAny<ProducerConfig>()));

            foreach (var mockProducer in producers)
            {
                sequence.Returns(mockProducer);
            }

            return this;
        }

        public IProducerFactory Get()
        {
            return _producerFactoryMock.Object;
        }
    }
}
