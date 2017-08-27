using Naruto.Dependency;
using Naruto.Dependency.Abstraction;
using Naruto.Plugins;
using Naruto.Reflection;

namespace Naruto
{
    /// <summary>
    /// 应用程序启动初始化工作
    /// </summary>
    public class ApplicationStartup
    {
        public IIocManager IocManager { get; }

        public ApplicationStartup(IIocManager iocManager)
        {
            IocManager = iocManager;
        }

        public virtual void Initialize()
        {
            PluginManager.Instance.Initialize();

            RegisterBuilders();
        }

        public void RegisterBuilders()
        {
            var builderTypes = AppDomainTypeFinder.Instance.OfType<IIocBuilder>();

            new AutofacBuilderBase(IocManager).Build();
        }
    }
}
