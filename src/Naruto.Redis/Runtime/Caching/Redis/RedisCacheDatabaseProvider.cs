using StackExchange.Redis;
using Naruto.Runtime.Configuration.Redis;
using Naruto.Redis.Providers;

namespace Naruto.Runtime.Caching.Redis
{
    public class RedisCacheDatabaseProvider : RedisDatabaseProvider
    {
        private readonly RedisOptions _options;

        public RedisCacheDatabaseProvider(RedisOptions options, IRedisConnectionProvider connectionProvider)
            : base(options, connectionProvider)
        {
            _options = options;
        }

        public override IDatabase GetDatabase()
        {
            return GetDatabase(_options.CacheOptions.DatabaseId);
        }
    }
}
