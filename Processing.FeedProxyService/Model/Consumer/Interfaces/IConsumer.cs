using System;
using System.Threading.Tasks;

namespace Processing.FeedProxyService.Model.Consumer.Interfaces
{
    public interface IConsumer : IDisposable
    {
        string Topic { get; }

        string Environment { get; }

        void Connect();

        void Subscribe(Func<byte[], Task> handler);
    }
}
