using Naruto.Dependency.Abstraction;
using StackExchange.Redis;
using System;

namespace Naruto.Redis.Providers
{
    public interface IRedisConnectionProvider : ISingletonDependency
    {
        Lazy<ConnectionMultiplexer> GetConnection();
    }
}
