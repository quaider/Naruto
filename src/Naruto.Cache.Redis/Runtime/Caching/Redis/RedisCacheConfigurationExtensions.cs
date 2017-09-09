using System;
using Naruto.Runtime.Configuration.Redis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Naruto.Dependency.Abstraction;

namespace Naruto.Runtime.Caching.Redis
{
    public static class RedisCacheConfigurationExtensions
    {
        public static INarutoServiceProvider AddRedisOptions(this INarutoServiceProvider provider)
        {
            return provider.AddRedisOptions(_ => { });
        }

        public static INarutoServiceProvider AddRedisOptions(this INarutoServiceProvider provider, Action<RedisCacheOptions> optionsAction)
        {
            provider.Services.AddSingleton<ICacheManager, RedisCacheManager>();

            var redisOptions = new RedisOptions();

            var section = provider.Configuration.GetSection("Naruto:Redis");
            section.Bind(redisOptions);

            provider.Services.AddSingleton<RedisOptions>(redisOptions);
            provider.Services.AddSingleton<RedisCacheOptions>(redisOptions.CacheOptions);

            optionsAction(redisOptions.CacheOptions);

            return provider;
        }
    }
}
