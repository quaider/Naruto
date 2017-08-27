using System;
using Microsoft.Extensions.DependencyInjection;
using Autofac.Extensions.DependencyInjection;
using Naruto.Dependency;
using Naruto.Dependency.Abstraction;
using Naruto.Dependency.Extensions;
using Naruto.Plugins;

namespace Naruto.AspNetCore
{
    public static class BootstrapServiceCollectionExtensions
    {
        public static IServiceProvider AddNaruto(this IServiceCollection services)
        {
            return services.AddNaruto(options => { });
        }

        public static IServiceProvider AddNaruto(this IServiceCollection services, Action<object> optionsAction)
        {
            optionsAction?.Invoke(new { });

            var startup = AddApplicationStartup();
            startup.startup.Initialize();

            //可做一些与services的集成操作
            var container = services.Populate(builder => builder.Populate(services));

            new PluginManager();

            //merge dependencies
            return new AutofacServiceProvider(container);
        }

        private static (ApplicationStartup startup, IocManager manager) AddApplicationStartup()
        {
            var iocManager = IocManager.Instance;
            ApplicationStartup startup = new ApplicationStartup(iocManager);
            iocManager.RegisterInstance(startup, LifetimeStyle.Singleton);
            return (startup, iocManager);
        }
    }
}
