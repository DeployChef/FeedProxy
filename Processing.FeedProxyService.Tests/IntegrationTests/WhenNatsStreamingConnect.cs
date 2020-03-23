using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Center.Processing.Framework.NatsStreaming;
using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Extensions;
using Ductus.FluentDocker.Model.Containers;
using Ductus.FluentDocker.Services;
using FluentAssertions;
using Processing.FeedProxyService.Model.Consumer.NatsImplementation;
using Processing.FeedProxyService.Tests.Utils;
using Xunit;

namespace Processing.FeedProxyService.Tests.IntegrationTests
{
    public class WhenNatsStreamingConnect
    {
        [Fact]
        public async Task Test()
        {
            var port = 4222;
            var subPort = 8222;

            using var container = Containers.CreateNatsStreamingContainer(port, subPort);

            container.Start();

            await Task.Delay(1000); //StartingNats

            var connectionConfiguration = new NatsStreamingConnectionConfigurationBuilder()
                .WithClusterId("test-cluster")
                .WithNatsUrl($"http://localhost:{port}/")
                .Build();

            var factory = new NatsStreamingConsumerFactory(connectionConfiguration);

            var natsConsumer = new NatsConsumer("test", "test", factory);

            Action act = () => natsConsumer.Connect();

            act.Should().NotThrow();
        }

        [Fact]
        public async Task Test2()
        {
            var port = 4222;
            var subPort = 8222;

            using var container = Containers.CreateNatsStreamingContainer(port, subPort);

            container.Start();

            await Task.Delay(1000); //StartingNats

            var connectionConfiguration = new NatsStreamingConnectionConfigurationBuilder()
                .WithClusterId("test-cluster")
                .WithNatsUrl($"http://localhost:{port}/")
                .Build();

            var factory = new NatsStreamingConsumerFactory(connectionConfiguration);

            var natsConsumer = new NatsConsumer("test", "test", factory);

            Action act = () => natsConsumer.Connect();

            act.Should().NotThrow();
        }
    }
}
