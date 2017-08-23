using Naruto.Dependency.Abstraction;

namespace Naruto.Runtime.Caching.Memory
{
    /// <summary>
    /// MemoryCache 管理实现
    /// </summary>
    public class MemoryCacheManager : CacheManagerBase
    {
        public MemoryCacheManager(IIocManager iocManager) : base(iocManager)
        {

        }

        protected override ICache CreateCacheImplementation(string name)
        {
            var cache = new MemoryCache(name);
            IocManager.RegisterInstance<MemoryCache>(name, cache, LifetimeStyle.Transient);

            //autofac must build first, so we can only return the objct we just created
            return cache;
        }
    }
}
