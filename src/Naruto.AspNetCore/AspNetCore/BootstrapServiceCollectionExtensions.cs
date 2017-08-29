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

            var startup = AddApplicationStartup();

            var mvcBuilder = services.AddMvc(opt =>
            {
                
            });

            startup.InitializePlugin((assemblies) =>
            {
                foreach (var ass in assemblies)
                {
                    mvcBuilder.PartManager.ApplicationParts.Add(new AssemblyPart(ass));
                }

            });

            startup.Initialize();

            //可做一些与services的集成操作
            var container = services.Populate(builder => builder.Populate(services));

            AppDomain.CurrentDomain.AssemblyResolve += (object sender, ResolveEventArgs args) =>
            {
                var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName == args.Name);
                if (assembly != null)
                    return assembly;

                assembly = IocManager.Instance.Resolve<ITypeFinder>().GetAssemblies().FirstOrDefault(a => a.FullName == args.Name);
                return assembly;
            };

            //merge dependencies
            return new AutofacServiceProvider(container);
        }

        private static ApplicationStartup AddApplicationStartup()
        {
            ApplicationStartup startup = new ApplicationStartup(IocManager.Instance);
            IocManager.Instance.RegisterInstance(startup, LifetimeStyle.Singleton);
            return startup;
        }
    }
}
