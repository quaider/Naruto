using System;
using Microsoft.Extensions.DependencyInjection;
using Autofac.Extensions.DependencyInjection;
using Naruto.Dependency;
using Naruto.Dependency.Abstraction;
using Naruto.Dependency.Extensions;
using Microsoft.AspNetCore.Hosting;
using Naruto.Constant;
using Naruto.Reflection;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

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

            ApplicationModel application = null;
            foreach (var controller in application.Controllers)
            {
                var type = controller.ControllerType.AsType();
            }

            services.Configure<MvcOptions>(opt =>
            {
                opt.Conventions
            });

            AddApplicationStartup(mvcBuilder);

            //可做一些与services的集成操作
            var container = services.Populate(builder => builder.Populate(services));

            //merge dependencies
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

        static void test()
        {
            Microsoft.AspNetCore.Mvc.ViewEngines.IViewEngine v;
            Microsoft.AspNetCore.Mvc.Razor.RazorViewEngine rzv;
            Microsoft.AspNetCore.Mvc.Razor.RazorViewEngineOptions opt;
        }
    }
}
