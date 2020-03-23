using Processing.FeedProxyService.Configs;

namespace Processing.FeedProxyService.Model.Producer.Interfaces
{
    public interface IProducerFactory
    {
        IProducer Create(string topic, ProducerConfig configuration);
    }
}
