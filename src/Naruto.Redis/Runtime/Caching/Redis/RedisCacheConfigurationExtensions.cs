using System;
using Naruto.Runtime.Configuration.Redis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Naruto.Dependency.Abstraction;
using Naruto.Redis.Providers;
using Naruto.Dependency;

namespace Naruto.Runtime.Caching.Redis
{
    public static class RedisCacheConfigurationExtensions
    {
        public static INarutoServiceProvider AddRedisOptions(this INarutoServiceProvider provider)
        {
            return provider.AddRedisOptions(option => option.UseRedisCache());
        }

        public static INarutoServiceProvider AddRedisOptions(this INarutoServiceProvider provider, Action<RedisOptions> optionsAction)
        {
            IocManager.Instance.Register<IRedisConnectionProvider, RedisConnectionProvider>(LifetimeStyle.Singleton);
            IocManager.Instance.Register<IRedisDatabaseProvider, RedisDatabaseProvider>(LifetimeStyle.Singleton);
            IocManager.Instance.Register<IRedisValueSerializer, DefaultRedisValueSerializer>(LifetimeStyle.Transient);

            var redisOptions = new RedisOptions();
            var section = provider.Configuration.GetSection("Naruto:Redis");
            section.Bind(redisOptions);

            optionsAction?.Invoke(redisOptions);

            if (redisOptions.EnableCache)
            {
                IocManager.Instance.Register<IRedisCacheDatabaseProvider, RedisCacheDatabaseProvider>(LifetimeStyle.Singleton);
                IocManager.Instance.Register<ICacheManager, RedisCacheManager>(LifetimeStyle.Singleton);
            }

            provider.Services.AddSingleton<RedisOptions>(redisOptions);

            return provider;
        }
    }
}
