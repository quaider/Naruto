using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Naruto.AspNetCore.Razor;
using Naruto.Constant;
using Naruto.Dependency;
using Naruto.Dependency.Abstraction;
using Naruto.Dependency.Extensions;

namespace Naruto
{
    internal class AspNetCoreServiceProvider : INarutoServiceProvider
    {
        private IContainer _container;

        public IServiceCollection Services { get; }

        public IConfigurationRoot Configuration { get; }

        public AspNetCoreServiceProvider(IConfigurationRoot configuration, IServiceCollection services)
        {
            Configuration = configuration;
            Services = services;
        }

        public AspNetCoreServiceProvider ConfigureService(Action configure)
        {
            var provider = Services.BuildServiceProvider();

            //set root path
            var hostingEnvironment = provider.GetRequiredService<IHostingEnvironment>();
            NarutoPath.AppBaseDirectory = hostingEnvironment.ContentRootPath;

            var mvcBuilder = Services.AddMvc(opt =>
            {
            });

            Services.AddPluginViewLocations();

            AddApplicationStartup(mvcBuilder);

            configure?.Invoke();

            return this;
        }

        public IServiceProvider Build()
        {
            //可做一些与services的集成操作
            _container = Services.Populate(builder => builder.Populate(Services));

            return new AutofacServiceProvider(_container);
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
                    builder.AddApplicationPart(ass);
                    //builder.PartManager.ApplicationParts.Add(new AssemblyPart(ass));
                }
            };

            startup.Initialize();

            return startup;
        }
    }
}
