using Naruto.Dependency.Abstraction;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Naruto.Runtime.Caching
{
    public abstract class CacheManagerBase : ICacheManager
    {
        protected readonly IIocManager IocManager;

        protected readonly ConcurrentDictionary<string, ICache> Caches;

        protected CacheManagerBase(IIocManager iocManager)
        {
            IocManager = iocManager;
            Caches = new ConcurrentDictionary<string, ICache>();
        }

        public IReadOnlyList<ICache> GetAllCaches()
        {
            //不可变集合
            return Caches.Values.ToImmutableList();
        }

        public ICache GetCache(string name)
        {
            Guard.NotNull(name, nameof(name));

            return Caches.GetOrAdd(name, (cacheName) =>
            {
                var cache = CreateCacheImplementation(cacheName);
                return cache;
            });
        }

        public ITypedCache<TKey, TValue> GetCache<TKey, TValue>(string name)
        {
            return GetCache(name).AsTyped<TKey, TValue>();
        }

        /// <summary>
        /// Used to create actual cache implementation.
        /// </summary>
        /// <param name="name">Name of the cache</param>
        /// <returns>Cache object</returns>
        protected abstract ICache CreateCacheImplementation(string name);

        public void Dispose()
        {
            foreach (var cache in Caches)
            {
                cache.Value.Dispose();
            }

            Caches.Clear();
        }
    }
}
