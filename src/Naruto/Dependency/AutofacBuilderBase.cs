using System.Linq;
using System.Reflection;
using Autofac;
using Naruto.Dependency.Abstraction;
using Naruto.Reflection;
using Naruto.Dependency.Installers;
using Naruto.Reflection.Extensions;


namespace Naruto.Dependency
{
    /// <summary>
    /// Autofac依赖注入构建器， 承担应用程序初始化依赖注入的工作
    /// </summary>
    internal class AutofacBuilderBase : IocBuilderBase
    {
        internal AutofacBuilderBase(IIocManager iocManager) : base(iocManager)
        {

        }

        protected override void RegisterCustomTypes(IIocManager iocManager)
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

            InstallInstallers();
        }

        private void InstallInstallers()
        {
            //保证在其他模块之前执行
            new NarutoInstaller().Install(IocManager.Instance, Finder);

            var installers = Finder.OfType<IDependencyInstaller>()
                .Where(f => f != typeof(NarutoInstaller))
                .Select(f => f.CreateInstance<IDependencyInstaller>())
                .OrderBy(f => f.Order);

            foreach (var installer in installers)
            {
                installer.Install(IocManager.Instance, Finder);
            }
        }
    }
}
