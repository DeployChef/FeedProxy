using System;
using System.Collections.Generic;
using System.Text;
using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;

namespace Processing.FeedProxyService.Tests.Utils
{
    public class Containers
    {
        public static IContainerService CreateNatsStreamingContainer(int port, int subPort) => new Builder().UseContainer()
                                                                                .UseImage("nats-streaming")
                                                                                .Command("-p", port.ToString())
                                                                                .ExposePort(port, port)
                                                                                .ExposePort(subPort, subPort)
                                                                                .Build();
    }
}
