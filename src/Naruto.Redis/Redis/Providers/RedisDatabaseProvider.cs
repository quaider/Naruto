using Naruto.Dependency;
using Naruto.Runtime.Caching.Redis;
using Naruto.Runtime.Configuration.Redis;
using StackExchange.Redis;

namespace Naruto.Redis.Providers
{
    internal class RedisDatabaseProvider : IRedisDatabaseProvider
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
            return GetDatabase(_options.DefaultDatabaseId);
        }

        public RedisService GetService()
        {
            return new RedisService(this, IocManager.Instance.Resolve<IRedisValueSerializer>());
        }

        protected IDatabase GetDatabase(int db)
        {
            return _connectionProvider.GetConnection().Value.GetDatabase(db);
        }
    }
}
