using Microsoft.Extensions.DependencyInjection;

namespace Processing.FeedProxyService.FeedProxies
{
    public static class ConfigsExtension
    {
        public static IServiceCollection AddFeedProxies(this IServiceCollection services)
        {
            services.AddTransient<IFeedProxy, FeedProxy>();
            services.AddHostedService<FeedProxiesRunner>();
            return services;
        }
    }
}
