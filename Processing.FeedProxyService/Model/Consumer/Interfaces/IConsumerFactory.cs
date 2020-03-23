using Processing.FeedProxyService.Configs;

namespace Processing.FeedProxyService.Model.Consumer.Interfaces
{
    public interface IConsumerFactory
    {
        IConsumer Create(string topic, ConsumerConfig configuration);
    }
}
