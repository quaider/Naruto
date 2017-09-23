using Naruto.Redis;
using System;

namespace Naruto.Runtime.Caching.Redis
{
    public class RedisCache : CacheBase
    {
        private readonly RedisService _service;

        public RedisCache(
            string name,
            RedisService redisService) : base(name)
        {
            _service = redisService;
        }

        public override object GetOrDefault(string key)
        {
            return _service.StringGet(key);
        }

        public override void Set(string key, object value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null)
        {
            if (value == null)
            {
                Remove(key);
                return;
            }

            _service.StringSet(key, value, absoluteExpireTime ?? slidingExpireTime ?? DefaultAbsoluteExpireTime ?? DefaultSlidingExpireTime);
        }

        public override void Remove(string key)
        {
            _service.Remove(key);
        }

        public override void Clear()
        {
            _service.Clear();
        }
    }
}
