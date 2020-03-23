using System;
using System.Threading.Tasks;

namespace Processing.FeedProxyService.Model.Producer.Interfaces
{
    public interface IProducer : IDisposable
    {
        string Topic { get; }

        string Environment { get; }

        void Connect();

        Task SendAsync(byte[] data);
    }
}
