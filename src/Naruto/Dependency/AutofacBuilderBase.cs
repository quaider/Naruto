using System;
using System.Reflection;
using Autofac;
using Naruto.Dependency.Abstraction;
using Naruto.Reflection;

namespace Naruto.Dependency
{
    /// <summary>
    /// Autofac依赖注入构建器， 承担应用程序初始化依赖注入的工作
    /// </summary>
    public abstract class AutofacBuilderBase : IocBuilderBase
    {
        protected AutofacBuilderBase(IIocManager iocManager, ITypeFinder finder) : base(iocManager, finder)
        {
        }

        protected override void Build(IIocManager iocManager, params Assembly[] assemblies)
        {
            RegisterInternal(assemblies);
        }

        private void RegisterInternal(params Assembly[] assemblies)
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
