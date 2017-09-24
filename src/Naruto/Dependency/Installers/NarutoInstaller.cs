using System;
using Autofac;
using Naruto.Configuration.Startup;
using Naruto.Dependency.Abstraction;
using Naruto.Reflection;
using Naruto.Runtime.Caching;
using Naruto.Runtime.Caching.Configuration;
using Naruto.Plugins;
using Naruto.Runtime.Caching.Memory;

namespace Naruto.Dependency.Installers
{
    internal class NarutoInstaller : IDependencyInstaller
    {
        public void Install(IIocManager manager, ITypeFinder finder)
        {
            manager.Register<IPluginFinder, PluginFinder>(LifetimeStyle.Scoped);
            manager.Register<IStartupConfiguration, StartupConfiguration>(LifetimeStyle.Singleton);
            manager.RegisterInstance<ICachingConfiguration>(InstallCaches(), LifetimeStyle.Singleton);
            manager.Register<ICacheManager, MemoryCacheManager>(LifetimeStyle.Singleton);
        }

        private CachingConfiguration InstallCaches()
        {
            var cachingConfig = new CachingConfiguration();

            cachingConfig.Configure(NarutoCacheNames.ApplicationSettings, cache =>
            {
                cache.DefaultSlidingExpireTime = TimeSpan.FromHours(8);
            });

            cachingConfig.Configure(NarutoCacheNames.UserSettings, cache =>
            {
                cache.DefaultSlidingExpireTime = TimeSpan.FromHours(20);
            });

            cachingConfig.Configure(NarutoCacheNames.LocalizationScripts, cache =>
            {
                cache.DefaultSlidingExpireTime = TimeSpan.FromDays(1);
            });

            cachingConfig.Configure(NarutoCacheNames.CommonDataCache, cache =>
            {
                cache.DefaultSlidingExpireTime = TimeSpan.FromDays(12);
            });

            return cachingConfig;
        }

        public int Order => int.MinValue;
    }
}
