using System;
using StackExchange.Redis;
using Naruto.Runtime.Configuration.Redis;

namespace Naruto.Redis.Providers
{
    internal class RedisConnectionProvider : IRedisConnectionProvider
    {
        private readonly RedisOptions _options;
        private readonly Lazy<ConnectionMultiplexer> _connectionMultiplexer;

        public RedisConnectionProvider(RedisOptions options)
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
