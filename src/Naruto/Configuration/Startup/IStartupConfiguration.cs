using Naruto.Runtime.Caching.Configuration;

namespace Naruto.Configuration.Startup
{
    /// <summary>
    /// 应用程序启动配置
    /// </summary>
    public interface IStartupConfiguration : IDictionaryBasedConfig
    {
        /// <summary>
        /// 缓存配置
        /// </summary>
        ICachingConfiguration Caching { get; }
    }
}
