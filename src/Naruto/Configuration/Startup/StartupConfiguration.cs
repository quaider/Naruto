using System;
using System.Collections.Generic;
using System.Text;
using Naruto.Runtime.Caching.Configuration;
using Naruto.Dependency.Abstraction;

namespace Naruto.Configuration.Startup
{
    internal class StartupConfiguration : DictionaryBasedConfig, IStartupConfiguration
    {
        public IIocManager IocManager { get; }

        public StartupConfiguration(IIocManager iocManager)
        {
            IocManager = iocManager;
        }

        /// <summary>
        /// 缓存配置
        /// </summary>
        public ICachingConfiguration Caching { get; private set; }

        public void Initialize(ICachingConfiguration caching)
        {
            Caching = caching;
        }
    }
}
