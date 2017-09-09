using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Autofac.Extensions.DependencyInjection;
using Naruto.Dependency;
using Naruto.Dependency.Abstraction;
using Naruto.Dependency.Extensions;
using Naruto.Constant;
using Naruto.AspNetCore.Razor;
using Naruto.Configuration.Startup;
using Naruto.Reflection.Extensions;
using Naruto.Runtime.Caching;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

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

        public static IApplicationBuilder UseNaruto(this IApplicationBuilder app)
        {
            var startupConfiguration = IocManager.Instance.Resolve<IStartupConfiguration>();
            //为了封闭StartupConfiguration给外部，更好的方案？
            startupConfiguration.GetType().InvokeMethod("Initialize", startupConfiguration, null);

            var cacheManager = IocManager.Instance.Resolve<ICacheManager>();
            var cache = cacheManager.GetCache(NarutoCacheNames.ApplicationSettings);

            app.UseMvc(routes =>
            {
                //areas
                routes.MapRoute(
                    name: "areaRoute",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            return app;
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
                    //builder.AddApplicationPart(ass);
                    builder.PartManager.ApplicationParts.Add(new AssemblyPart(ass));
                }
            };

            startup.Initialize();

            return startup;
        }

    }
}
