using Naruto.Dependency.Abstraction;
using System;
using System.Collections.Generic;
using System.Text;

namespace Naruto.Runtime.Caching.Redis
{
    public class RedisCacheManager : CacheManagerBase
    {
        public RedisCacheManager(IIocManager iocManager)
            : base(iocManager)
        {
            
        }

        protected override ICache CreateCacheImplementation(string name)
        {
            var redisCacheProvider = IocManager.Resolve<IRedisCacheDatabaseProvider>();
            var serializer = IocManager.Resolve<IRedisCacheSerializer>();

            return new RedisCache(name, redisCacheProvider, serializer);
        }
    }
}
