using Naruto.Dependency.Abstraction;
using System;
using System.Reflection;
using Autofac;

namespace Naruto.Dependency
{
    /// <summary>
    /// Autofac依赖注入构建器， 承担应用程序初始化依赖注入的工作
    /// </summary>
    public class AutofacBuilderBase : IocBuilderBase
    {
        public AutofacBuilderBase(IIocManager iocManager) : base(iocManager)
        {
        }

        protected override IServiceProvider Build(IIocManager iocManager, params Assembly[] assemblies)
        {
            RegisterInternal(assemblies);
            return new DefaultServiceProvider(IocManager.Instance);
        }

        private static void RegisterInternal(params Assembly[] assemblies)
        {
            //Register the type as providing all of its public interfaces as services (excluding IDisposable).
            IocManager.Instance.ContainerBuilder
                .RegisterAssemblyTypes(assemblies)
                .Where(t => typeof(ITransientDependency).IsAssignableFrom(t))
                .AsImplementedInterfaces()
                .As<ITransientDependency>()
                .AsSelf();

            IocManager.Instance.ContainerBuilder
               .RegisterAssemblyTypes(assemblies)
               .Where(t => typeof(IScopeDependency).IsAssignableFrom(t))
               .AsImplementedInterfaces()
               .As<IScopeDependency>()
               .AsSelf();

            IocManager.Instance.ContainerBuilder
               .RegisterAssemblyTypes(assemblies)
               .Where(t => typeof(ISingletonDependency).IsAssignableFrom(t))
               .AsImplementedInterfaces()
               .As<ISingletonDependency>()
               .AsSelf();
        }
    }
}
