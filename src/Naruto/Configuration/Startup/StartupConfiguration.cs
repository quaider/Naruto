using Naruto.Runtime.Caching.Configuration;
using Naruto.Dependency.Abstraction;

namespace Naruto.Configuration.Startup
{
    internal class StartupConfiguration : DictionaryBasedConfig, IStartupConfiguration
    {
        public IIocManager IocManager { get; }

        public StartupConfiguration(IIocManager manager)
        {
            IocManager = manager;
        }

        /// <summary>
        /// 缓存配置
        /// </summary>
        public ICachingConfiguration Caching { get; set; }

        public void Initialize()
        {
            Caching = IocManager.Resolve<ICachingConfiguration>();
        }
    }
}
