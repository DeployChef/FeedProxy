using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Processing.FeedProxyService.Configs;
using Processing.FeedProxyService.FeedProxies;
using Processing.FeedProxyService.Model.Consumer.Interfaces;
using Processing.FeedProxyService.Model.Consumer.NatsImplementation;
using Processing.FeedProxyService.Model.Producer.Interfaces;
using Processing.FeedProxyService.Model.Producer.NatsImplementation;
using Prometheus;

namespace Processing.FeedProxyService
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks();

            services.AddFeedProxies();

            services.AddSingleton<IConsumerFactory, NatsConsumerFactory>();
            services.AddSingleton<IProducerFactory, NatsProducerFactory>();

            services.Configure<TopicsConfig>(Configuration.GetSection("Topics"));
            services.Configure<ConsumerConfig>(Configuration.GetSection("Consumer"));
            services.Configure<ProducerConfigs>(Configuration.GetSection("Producers"));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHealthChecks("/hc");

            app.UseRouting();
            app.UseHttpMetrics();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapMetrics();
            });
        }
    }
}
