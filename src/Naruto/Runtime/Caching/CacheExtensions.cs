using System;
using System.Threading.Tasks;

namespace Naruto.Runtime.Caching
{
    /// <summary>
    /// 对 <see cref="ICache"/> 的扩展
    /// </summary>
    public static class CacheExtensions
    {
        public static object Get(this ICache cache, string key, Func<object> factory)
        {
            return cache.Get(key, k => factory());
        }

        public static Task<object> GetAsync(this ICache cache, string key, Func<Task<object>> factory)
        {
            return cache.GetAsync(key, k => factory());
        }

        /// <summary>
        /// ICache对象转换为ITypedCache对象
        /// </summary>
        /// <returns><see cref="ITypedCache{TKey, TValue}"/> 对象</returns>
        public static ITypedCache<TKey, TValue> AsTyped<TKey, TValue>(this ICache cache)
        {
            return new TypedCache<TKey, TValue>(cache);
        }

        public static TValue Get<TKey, TValue>(this ICache cache, TKey key, Func<TKey, TValue> factory)
        {
            return (TValue)cache.Get(key.ToString(), k => factory(key));
        }

        public static TValue Get<TKey, TValue>(this ICache cache, TKey key, Func<TValue> factory)
        {
            return cache.Get(key, k => factory());
        }

		public static async Task<TValue> GetAsync<TKey, TValue>(this ICache cache, TKey key, Func<TKey, Task<TValue>> factory)
		{
			var value = await cache.GetAsync(key.ToString(), async keyAsString =>
			{
				return await factory(key);
			});

			return (TValue)value;
		}

		public static Task<TValue> GetAsync<TKey, TValue>(this ICache cache, TKey key, Func<Task<TValue>> factory)
		{
			return cache.GetAsync(key, k => factory());
		}

		public static TValue GetOrDefault<TKey, TValue>(this ICache cache, TKey key)
		{
			var value = cache.GetOrDefault(key.ToString());
			if (value == null)
			{
				return default(TValue);
			}

			return (TValue)value;
		}

		public static async Task<TValue> GetOrDefaultAsync<TKey, TValue>(this ICache cache, TKey key)
		{
			var value = await cache.GetOrDefaultAsync(key.ToString());
			if (value == null)
			{
				return default(TValue);
			}

			return (TValue)value;
		}
    }
}
