using Naruto.Dependency.Abstraction;
using Naruto.Dependency.Installers;
using Naruto.Reflection;
using Naruto.Reflection.Extensions;

namespace Naruto
{
    /// <summary>
    /// 应用程序启动初始化工作
    /// </summary>
    public class ApplicationStartup
    {
        public ITypeFinder Finder { get; }

        public IIocManager IocManager { get; }

        public ApplicationStartup(IIocManager iocManager)
        {
            IocManager = iocManager;
            Finder = new AppDomainTypeFinder();

            IocManager.RegisterInstance(Finder, LifetimeStyle.Singleton);
        }

        public virtual void Initialize()
        {
            new NarutoInstaller().Install(IocManager);

            RegisterBuilders();
        }

        public void RegisterBuilders()
        {
            var builderTypes = Finder.OfType<IIocBuilder>();
            foreach (var type in builderTypes)
            {
                var builder = type.CreateInstance<IIocBuilder>(IocManager, Finder);
                if (builder != null) builder.Build();
            }
        }
    }
}
