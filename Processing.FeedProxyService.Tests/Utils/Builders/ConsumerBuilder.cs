using Processing.FeedProxyService.Tests.Models;
using System;

namespace Processing.FeedProxyService.Tests.Utils.Builders
{
    public class ConsumerBuilder
    {
        private FakeConsumer _consumer;

        public ConsumerBuilder(string environment)
        {
            _consumer = new FakeConsumer(environment);
        }

        public ConsumerBuilder SetDelay(int delay)
        {
            _consumer.Delay = delay;
            return this;
        }

        public ConsumerBuilder UseConnectException(Exception exception)
        {
            _consumer.ConnectException = exception;
            return this;
        }

        public ConsumerBuilder SetEventCounts(int counts)
        {
            _consumer.EventCounts = counts;
            return this;
        }

        public FakeConsumer Get()
        {
            return _consumer;
        }
    }
}
