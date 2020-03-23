using Processing.FeedProxyService.Model.Consumer.Interfaces;
using System;
using System.Threading.Tasks;

namespace Processing.FeedProxyService.Tests.Models
{
    public class FakeConsumer : IConsumer
    {
        public FakeConsumer(string environment)
        {
            Environment = environment;
        }

        public string Topic { get; }

        public string Environment { get; }

        public int Delay { get; set; } = 0;

        public Exception ConnectException { get; set; } = null;

        public int EventCounts { get; set; } = -1;

        public bool SendedAll { get; private set; }


        public void Connect()
        {
            if (ConnectException != null)
                throw ConnectException;
        }

        public void Subscribe(Func<byte[], Task> handler)
        {
            Task.Run(async () =>
            {
                var i = 0;
                while (EventCounts == -1 || i < EventCounts)
                {
                    await handler(new byte[0]);
                    await Task.Delay(Delay);
                    i++;
                }

                SendedAll = true;
            });
        }

        public void Dispose()
        { }
    }
}
