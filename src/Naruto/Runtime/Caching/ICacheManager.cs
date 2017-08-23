using System;
using System.Collections.Generic;

namespace Naruto.Runtime.Caching
{
    /// <summary>
    /// cache manager应该以单例形式使用，管理及跟踪 <see cref="ICache"/> 对象
    /// </summary>
    public interface ICacheManager : IDisposable
    {
        /// <summary>
        /// 获取所有 <see cref="ICache"/>
        /// </summary>
        /// <returns></returns>
        IReadOnlyList<ICache> GetAllCaches();

        /// <summary>
        /// 根据缓存名称获取指定缓存 <see cref="ICache"/>
        /// </summary>
        /// <param name="name">缓存名称</param>
        /// <returns><see cref="ICache"/></returns>
        ICache GetCache(string name);

        /// <summary>
        /// 获取强类型的 <see cref="ICache"/>
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="cacheManager"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        ITypedCache<TKey, TValue> GetCache<TKey, TValue>(string name);
    }
}
