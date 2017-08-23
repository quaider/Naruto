using System;
using System.Collections.Generic;

namespace Naruto.Collections.Extensions
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// 尝试从字典中返回指定类型的值
        /// </summary>
        /// <typeparam name="T">要返回的类型</typeparam>
        /// <param name="dictionary">字典源</param>
        /// <param name="key">key</param>
        /// <param name="value">存储转换后的值(失败则返回类型的默认值)</param>
        /// <returns>指示转换是否成功</returns>
        public static bool TryGetValue<T>(this IDictionary<string, object> dictionary, string key, out T value)
        {
            object valueObj;
            if (dictionary.TryGetValue(key, out valueObj) && valueObj is T)
            {
                value = (T)valueObj;
                return true;
            }

            value = default(T);
            return false;
        }

        /// <summary>
        /// 返回指定key的值， 没有则返回类型的默认值
        /// </summary>
        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            TValue obj;
            return dictionary.TryGetValue(key, out obj) ? obj : default(TValue);
        }

        /// <summary>
        /// 返回指定key的值，没有则使用指定工厂创建key的value, 并附加于字典上
        /// </summary>
        /// <param name="factory">value创建工厂</param>
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> factory)
        {
            TValue obj;
            if (dictionary.TryGetValue(key, out obj))
            {
                return obj;
            }

            return dictionary[key] = factory(key);
        }

        /// <summary>
        /// 返回指定key的值，没有则使用指定工厂创建key的value, 并附加于字典上
        /// </summary>
        /// <param name="factory">value创建工厂</param>
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> factory)
        {
            return dictionary.GetOrAdd(key, k => factory());
        }
    }
}
