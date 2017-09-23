using Naruto.Runtime.Configuration.Redis;
using StackExchange.Redis;

namespace Naruto.Redis.Providers
{
    public class RedisDatabaseProvider : IRedisDatabaseProvider
    {
        private readonly RedisOptions _options;
        private readonly IRedisConnectionProvider _connectionProvider;

        public RedisDatabaseProvider(RedisOptions options, IRedisConnectionProvider connectionProvider)
        {
            _options = options;
            _connectionProvider = connectionProvider;
        }

        public virtual IDatabase GetDatabase()
        {
            return _connectionProvider.GetConnection().Value.GetDatabase(_options.DefaultDatabaseId);
        }
    }
}
