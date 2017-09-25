using System;
using Microsoft.Extensions.DependencyInjection;
using Naruto.Dependency;
using Naruto.Configuration.Startup;
using Naruto.Reflection.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Naruto.Dependency.Abstraction;

namespace Naruto.AspNetCore
{
    public static class BootstrapServiceCollectionExtensions
    {
        public static INarutoServiceProvider AddNaruto(this IServiceCollection services, IConfigurationRoot configuration)
        {
            return services.AddNaruto(configuration, () => { });
        }

        public static INarutoServiceProvider AddNaruto(this IServiceCollection services, IConfigurationRoot configuration, Action configure)
        {
            var bootstrap = new AspNetCoreServiceProvider(configuration, services);

            return bootstrap.ConfigureService(configure);
        }

        public static IApplicationBuilder UseNaruto(this IApplicationBuilder app)
        {
            var startupConfiguration = IocManager.Instance.Resolve<IStartupConfiguration>();
            //为了封闭StartupConfiguration给外部，更好的方案？
            startupConfiguration.GetType().InvokeMethod("Initialize", startupConfiguration);

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



    }
}
