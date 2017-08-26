using Naruto.Dependency;
using Naruto.Dependency.Abstraction;
using Naruto.Reflection;

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
            RegisterBuilders();
        }

        public void RegisterBuilders()
        {
            var builderTypes = Finder.OfType<IIocBuilder>();

            new AutofacBuilderBase(IocManager, Finder).Build();
        }
    }
}
