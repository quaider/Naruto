using System;
using StackExchange.Redis;
using Naruto.Json;
using Naruto.Dependency.Abstraction;

namespace Naruto.Runtime.Caching.Redis
{
    public class DefaultRedisValueSerializer : IRedisValueSerializer, ITransientDependency
    {
        public object Deserialize(RedisValue objbyte)
        {
            return JsonSerializationHelper.DeserializeWithType(objbyte);
        }

        public T Deserialize<T>(RedisValue objbyte)
        {
            return JsonSerializationHelper.DeserializeWithType<T>(objbyte);
        }

        public string Serialize(object value, Type type)
        {
            return JsonSerializationHelper.SerializeWithType(value, type);
        }
    }
}
