using Autofac;
using Naruto.Configuration.Startup;
using Naruto.Dependency.Abstraction;
using Naruto.Runtime.Caching;
using Naruto.Runtime.Caching.Configuration;
using System;

namespace Naruto.Dependency.Installers
{
    internal class NarutoInstaller
    {
        public void Install(IIocManager manager)
        {
            manager.Register<ICachingConfiguration, CachingConfiguration>(LifetimeStyle.Singleton);
            IocManager.Instance.Register<IStartupConfiguration>(ctx =>
            {
                var startupConfig = new StartupConfiguration(manager);
                var cachingConfig = ctx.Resolve<ICachingConfiguration>();

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

                startupConfig.Initialize(cachingConfig);

                return startupConfig;

            }, LifetimeStyle.Singleton);

        }
    }
}
