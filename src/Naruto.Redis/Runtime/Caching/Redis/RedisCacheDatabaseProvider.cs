using StackExchange.Redis;
using Naruto.Runtime.Configuration.Redis;
using Naruto.Redis.Providers;
using Naruto.Redis;
using Naruto.Dependency;

namespace Naruto.Runtime.Caching.Redis
{
    internal class RedisCacheDatabaseProvider : IRedisCacheDatabaseProvider
    {
        private readonly RedisOptions _options;
        private readonly IRedisConnectionProvider _provider;

        public RedisCacheDatabaseProvider(RedisOptions options, IRedisConnectionProvider connectionProvider)
        {
            _options = options;
            _provider = connectionProvider;
        }

        public IDatabase GetDatabase()
        {
            return _provider.GetConnection().Value.GetDatabase(_options.CacheOptions.DatabaseId);
        }

        public RedisService GetService()
        {
            return new RedisService(this, IocManager.Instance.Resolve<IRedisValueSerializer>());
        }
    }
}
