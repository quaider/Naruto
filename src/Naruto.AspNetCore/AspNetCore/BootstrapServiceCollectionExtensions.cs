using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Hosting;
using Autofac.Extensions.DependencyInjection;
using Naruto.Dependency;
using Naruto.Dependency.Abstraction;
using Naruto.Dependency.Extensions;
using Naruto.Constant;
using Naruto.AspNetCore.Razor;

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
            var provider = services.BuildServiceProvider();

            //set root path
            var hostingEnvironment = provider.GetRequiredService<IHostingEnvironment>();
            NarutoPath.AppBaseDirectory = hostingEnvironment.ContentRootPath;

            var mvcBuilder = services.AddMvc(opt =>
            {
            });

            services.AddPluginViewLocations();

            AddApplicationStartup(mvcBuilder);

            //可做一些与services的集成操作
            var container = services.Populate(builder => builder.Populate(services));

            return new AutofacServiceProvider(container);
        }

        private static ApplicationStartup AddApplicationStartup(IMvcBuilder builder)
        {
            ApplicationStartup startup = new ApplicationStartup(IocManager.Instance);
            IocManager.Instance.RegisterInstance(startup, LifetimeStyle.Singleton);

            //因为ApplicationManager在MVC包中，所以Naruto基础库不应该依赖此包
            startup.OnInitializePlugin += (assemblies) =>
            {
                foreach (var ass in assemblies)
                {
                    builder.PartManager.ApplicationParts.Add(new AssemblyPart(ass));
                }
            };

            startup.Initialize();

            return startup;
        }
    }
}
