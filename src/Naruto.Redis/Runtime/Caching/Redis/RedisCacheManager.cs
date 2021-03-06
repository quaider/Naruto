﻿using Naruto.Dependency.Abstraction;
using Naruto.Redis;

namespace Naruto.Runtime.Caching.Redis
{
    public class RedisCacheManager : CacheManagerBase
    {
        private RedisService _redisService;

        public RedisCacheManager(IIocManager iocManager)
            : base(iocManager)
        {
            var redisCacheProvider = IocManager.Resolve<RedisCacheDatabaseProvider>();
            _redisService = redisCacheProvider.GetService();
        }

        protected override ICache CreateCacheImplementation(string name)
        {
            return new RedisCache(name, _redisService);
        }
    }
}
