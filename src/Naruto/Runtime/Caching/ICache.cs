using System;
using System.Threading.Tasks;

namespace Naruto.Runtime.Caching
{
    /// <summary>
    /// Cache操作接口，from abp
    /// </summary>
    public interface ICache : IDisposable
    {
        /// <summary>
        /// 缓存的唯一名称
        /// </summary>
        /// <value>The name.</value>
        string Name { get; }

        /// <summary>
        /// 缓存项的默认滑动过期时间
        /// 默认值: 60 分钟
        /// 可从配置中修改
        /// </summary>
        TimeSpan DefaultSlidingExpireTime { get; set; }

        /// <summary>
        /// 缓存项的绝对过期时间
        /// 默认值: null (没用到).
        /// </summary>
        TimeSpan? DefaultAbsoluteExpireTime { get; set; }

        /// <summary>
        /// 获取缓存项
        /// </summary>
        /// <returns>缓存项</returns>
        /// <param name="key">Key</param>
        /// <param name="factroy">如果没命中，则使用该工厂创建缓存项</param>
        object Get(string key, Func<string, object> factroy);

        /// <summary>
        /// 获取缓存项
        /// </summary>
        /// <returns>缓存项</returns>
        /// <param name="key">Key</param>
        /// <param name="factory">如果没命中，则使用该工厂创建缓存项</param>
        Task<object> GetAsync(string key, Func<string, Task<object>> factory);

        /// <summary>
        /// 获取缓存项，如果不存在则返回null
        /// </summary>
        /// <returns>缓存项或null</returns>
        /// <param name="key">Key</param>
        object GetOrDefault(string key);

        /// <summary>
        /// 获取缓存项，如果不存在则返回null
        /// </summary>
        /// <returns>缓存项或null</returns>
        /// <param name="key">Key</param>
        Task<object> GetOrDefaultAsync(string key);

        /// <summary>
        /// 设置缓存(存在则覆盖，不存在则添加)
        /// 最多只能设置一种过期方式 (<paramref name="slidingExpireTime"/> or <paramref name="absoluteExpireTime"/>)
        /// 如果都没设置，则使用 <see cref="DefaultAbsoluteExpireTime"/> 当其不为null时, 
        /// 否则使用<see cref="DefaultSlidingExpireTime"/>
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">缓存项的值</param>
        /// <param name="slidingExpireTime">滑动过期时间</param>
        /// <param name="absoluteExpireTime">绝对过期时间</param>
        void Set(string key, object value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null);

        /// <summary>
        /// 设置缓存(存在则覆盖，不存在则添加)
        /// 最多只能设置一种过期方式 (<paramref name="slidingExpireTime"/> or <paramref name="absoluteExpireTime"/>)
        /// 如果都没设置，则使用 <see cref="DefaultAbsoluteExpireTime"/> 当其不为null时, 
        /// 否则使用<see cref="DefaultSlidingExpireTime"/>
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">缓存项的值</param>
        /// <param name="slidingExpireTime">滑动过期时间</param>
        /// <param name="absoluteExpireTime">绝对过期时间</param>
        Task SetAsync(string key, object value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null);

        /// <summary>
        /// 删除指定key的缓存项
        /// </summary>
        /// <param name="key">Key</param>
        void Remove(string key);

        /// <summary>
        /// 删除指定key的缓存项
        /// </summary>
        /// <param name="key">Key</param>
        Task RemoveAsync(string key);

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