using System;
using StackExchange.Redis;
using Naruto.Json;

namespace Naruto.Runtime.Caching.Redis
{
    public class DefaultRedisCacheSerializer : IRedisCacheSerializer
    {
        public object Deserialize(RedisValue objbyte)
        {
            return JsonSerializationHelper.DeserializeWithType(objbyte);
        }

        public string Serialize(object value, Type type)
        {
            return JsonSerializationHelper.SerializeWithType(value, type);
        }
    }
}
