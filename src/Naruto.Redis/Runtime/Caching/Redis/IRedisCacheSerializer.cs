using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Naruto.Runtime.Caching.Redis
{
    /// <summary>
    /// 用于从redis存储数据和获取数据时，提供序列化操作
    /// </summary>
    public interface IRedisCacheSerializer
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="objbyte">RedisValue</param>
        /// <returns>返回一个对象</returns>
        object Deserialize(RedisValue objbyte);

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="value">待序列化的实例</param>
        /// <param name="type">待序列化的实例的类型</param>
        /// <returns>返回一个可存储与redis的字符串</returns>
        string Serialize(object value, Type type);
    }
}
