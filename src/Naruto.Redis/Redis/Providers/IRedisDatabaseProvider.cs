using Naruto.Dependency.Abstraction;
using StackExchange.Redis;

namespace Naruto.Redis.Providers
{
    public interface IRedisDatabaseProvider : ISingletonDependency
    {
        /// <summary>
        /// <see cref="IDatabase"/>
        /// </summary>
        IDatabase GetDatabase();
    }
}
