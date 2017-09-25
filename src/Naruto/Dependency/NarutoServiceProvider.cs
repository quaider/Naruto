using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Naruto.Dependency.Abstraction;
using Naruto.Dependency.Extensions;
using Autofac;
using Autofac.Extensions.DependencyInjection;

namespace Naruto.Dependency
{
    /// <summary>
    /// 用于测试目的，请勿使用
    /// </summary>
    public class NarutoServiceProvider : INarutoServiceProvider
    {
        private IContainer _container;

        public IServiceCollection Services { get; }

        public IConfigurationRoot Configuration { get; }

        public NarutoServiceProvider(IConfigurationRoot configuration, IServiceCollection services)
        {
            Configuration = configuration;
            Services = services;
        }

        public IServiceProvider Build()
        {
            AddApplicationStartup();

            _container = Services.Populate(builder => builder.Populate(Services));

            return new AutofacServiceProvider(_container);
        }

        private static ApplicationStartup AddApplicationStartup()
        {
            ApplicationStartup startup = new ApplicationStartup(IocManager.Instance);
            startup.OnInitializePlugin += (assemblies) =>
            {
            };

            IocManager.Instance.RegisterInstance(startup, LifetimeStyle.Singleton);

            startup.Initialize();

            return startup;
        }
    }
}
