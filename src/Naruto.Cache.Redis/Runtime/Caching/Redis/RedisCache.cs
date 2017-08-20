using Naruto.Runtime.Caching;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Naruto.Runtime.Caching.Redis
{
    public class RedisCache : CacheBase
    {
        private readonly IDatabase _database;
        private readonly IRedisCacheSerializer _serializer;

        public RedisCache(
            string name,
            IRedisCacheDatabaseProvider redisCacheDatabaseProvider,
            IRedisCacheSerializer redisCacheSerializer) : base(name)
        {
            _database = redisCacheDatabaseProvider.GetDatabase();
            _serializer = redisCacheSerializer;
        }

        public override object GetOrDefault(string key)
        {
            var objbyte = _database.StringGet(key);
            return objbyte.HasValue ? Deserialize(objbyte) : null;
        }

        public override void Set(string key, object value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null)
        {
            if (value == null)
            {
                throw new Exception("Can not insert null values to the cache!");
            }

            var type = value.GetType();
            _database.StringSet(
                key,
                Serialize(value, type),
                absoluteExpireTime ?? slidingExpireTime ?? DefaultAbsoluteExpireTime ?? DefaultSlidingExpireTime
            );
        }

        public override void Remove(string key)
        {
            _database.KeyDelete(key);
        }

        public override void Clear()
        {
            _database.KeyDeleteWithPrefix("*");
        }

        protected virtual string Serialize(object value, Type type)
        {
            return _serializer.Serialize(value, type);
        }

        protected virtual object Deserialize(RedisValue objbyte)
        {
            return _serializer.Deserialize(objbyte);
        }
    }
}
