using Naruto.Runtime.Configuration.Redis;
using StackExchange.Redis;
using System;

namespace Naruto.Runtime.Caching.Redis
{
    public class RedisCacheDatabaseProvider : IRedisCacheDatabaseProvider
    {
        private readonly RedisCacheOptions _options;
        private readonly Lazy<ConnectionMultiplexer> _connectionMultiplexer;

        public RedisCacheDatabaseProvider(RedisCacheOptions options)
        {
            _options = options;
            _connectionMultiplexer = new Lazy<ConnectionMultiplexer>(CreateConnectionMultiplexer);
        }

        public IDatabase GetDatabase()
        {
            return _connectionMultiplexer.Value.GetDatabase(_options.DatabaseId);
        }

        private ConnectionMultiplexer CreateConnectionMultiplexer()
        {
            return ConnectionMultiplexer.Connect(_options.ConnectionString);
        }
    }
}
