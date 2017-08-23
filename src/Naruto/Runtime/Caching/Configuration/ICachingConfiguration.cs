using System;
using System.Collections.Generic;

namespace Naruto.Runtime.Caching.Configuration
{
    /// <summary>
    /// 用户配置系统的缓存(应该单例注册)
    /// </summary>
    public interface ICachingConfiguration
    {
        /// <summary>
        /// 所有已注册的缓存配置信息
        /// </summary>
        IReadOnlyList<CacheConfigurator> Configurators { get; }

        /// <summary>
        /// 用于配置所有缓存
        /// </summary>
        /// <param name="configure">
        /// 用于统一配置缓存
        /// </param>
        void ConfigureAll(Action<ICache> configure);

        /// <summary>
        /// 用于配置指定所有缓存
        /// </summary>
        /// <param name="cacheName">缓存名称</param>
        /// <param name="initAction">
        /// 用于配置单个缓存
        /// </param>
        void Configure(string cacheName, Action<ICache> initAction);
    }
}
