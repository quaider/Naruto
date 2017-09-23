using System;
using StackExchange.Redis;
using Naruto.Runtime.Configuration.Redis;

namespace Naruto.Redis.Providers
{
    internal class RedisConnectionProvider : IRedisConnectionProvider
    {
        private readonly RedisCacheOptions _options;
        private readonly Lazy<ConnectionMultiplexer> _connectionMultiplexer;

        public RedisConnectionProvider(RedisCacheOptions options)
        {
            _options = options;
            _connectionMultiplexer = new Lazy<ConnectionMultiplexer>(CreateConnectionMultiplexer);
        }

        public Lazy<ConnectionMultiplexer> GetConnection()
        {
            return _connectionMultiplexer;
        }

        private ConnectionMultiplexer CreateConnectionMultiplexer()
        {
            return ConnectionMultiplexer.Connect(_options.ConnectionString);
        }
    }
}
