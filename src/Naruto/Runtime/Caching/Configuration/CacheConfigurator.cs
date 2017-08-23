using System;

namespace Naruto.Runtime.Caching.Configuration
{
    /// <summary>
    /// 单个缓存配置实例
    /// </summary>
    public class CacheConfigurator
    {
        public string CacheName { get; private set; }

        public Action<ICache> Configure { get; private set; }

        public CacheConfigurator(Action<ICache> configure)
        {
            Configure = configure;
        }

        public CacheConfigurator(string cacheName, Action<ICache> configure)
        {
            CacheName = cacheName;
            Configure = configure;
        }
    }
}
