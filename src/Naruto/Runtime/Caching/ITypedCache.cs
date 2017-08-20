using System;
using System.Threading.Tasks;

namespace Naruto.Runtime.Caching
{
    /// <summary>
    /// 缓存操作泛型实现
    /// </summary>
    /// <typeparam name="TKey">键类型</typeparam>
    /// <typeparam name="TValue">键值类型</typeparam>
    public interface ITypedCache<TKey, TValue> : IDisposable
    {
        /// <summary>
        /// 缓存唯一名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 默认滑动过期时间
        /// </summary>
        /// <value>The default sliding expire time.</value>
        TimeSpan DefaultSlidingExpireTime { get; set; }

        /// <summary>
        /// 内部缓存
        /// </summary>
        ICache InternalCache { get; }

        /// <summary>
        /// 获取缓存项
        /// </summary>
        /// <returns>缓存项</returns>
        /// <param name="key">Key</param>
        /// <param name="factory">如果没命中，则使用该工厂创建缓存项</param>
        TValue Get(TKey key, Func<TKey, TValue> factory);

        /// <summary>
        /// 获取缓存项
        /// </summary>
        /// <returns>缓存项</returns>
        /// <param name="key">Key</param>
        /// <param name="factory">如果没命中，则使用该工厂创建缓存项</param>
        Task<TValue> GetAsync(TKey key, Func<TKey, Task<TValue>> factory);

        /// <summary>
        /// 获取缓存项，如果不存在则返回null
        /// </summary>
        /// <returns>缓存项或null</returns>
        /// <param name="key">Key</param>
        TValue GetOrDefault(TKey key);

        /// <summary>
        /// 获取缓存项，如果不存在则返回null
        /// </summary>
        /// <returns>缓存项或null</returns>
        /// <param name="key">Key</param>
        Task<TValue> GetOrDefaultAsync(TKey key);

        /// <summary>
        /// 设置缓存(存在则覆盖，不存在则添加)
        /// </summary>
        /// <returns>The set.</returns>
        /// <param name="key">Key</param>
        /// <param name="value">缓存项的值</param>
        /// <param name="slidingExpireTime">滑动过期时间</param>
        void Set(TKey key, TValue value, TimeSpan? slidingExpireTime = null);

        /// <summary>
        /// 设置缓存(存在则覆盖，不存在则添加)
        /// </summary>
        /// <returns>The set.</returns>
        /// <param name="key">Key</param>
        /// <param name="value">缓存项的值</param>
        /// <param name="slidingExpireTime">滑动过期时间</param>
        Task SetAsync(TKey key, TValue value, TimeSpan? slidingExpireTime = null);

        /// <summary>
        /// 删除指定key的缓存项
        /// </summary>
        /// <param name="key">Key</param>
        void Remove(TKey key);

        /// <summary>
        /// 删除指定key的缓存项
        /// </summary>
        /// <param name="key">Key</param>
        Task RemoveAsync(TKey key);

        /// <summary>
        /// 清除所有缓存项
        /// </summary>
        void Clear();

        /// <summary>
        /// 清除所有缓存项
        /// </summary>
        Task ClearAsync();
    }
}
