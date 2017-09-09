using System;
using Naruto.Runtime.Configuration.Redis;
using Microsoft.Extensions.DependencyInjection;

namespace Naruto.Runtime.Caching.Redis
{
    public static class RedisCacheConfigurationExtensions
    {
        public static void AddRedis(this IServiceCollection services)
        {
            services.AddRedis(options => { });
        }

        public static void AddRedis(this IServiceCollection services, Action<RedisCacheOptions> optionsAction)
        {
            services.AddSingleton<ICacheManager, RedisCacheManager>();

            optionsAction(null);
        }
    }
}
