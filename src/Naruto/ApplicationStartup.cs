using System;
using System.Collections.Generic;
using System.Reflection;
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

        public void Initialize()
        {
            RegisterBuilders();
        }

        public void InitializePlugin(Action<IReadOnlyList<Assembly>> action)
        {
            PluginManager.Instance.Initialize();
            action(PluginManager.Instance.GetManualReferencedAssembly());
        }

        public void RegisterBuilders()
        {
            var builderTypes = AppDomainTypeFinder.Instance.OfType<IIocBuilder>();

            new AutofacBuilderBase(IocManager).Build();
        }
    }
}
