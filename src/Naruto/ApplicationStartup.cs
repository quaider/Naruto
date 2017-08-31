using System;
using System.Collections.Generic;
using System.Linq;
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

        /// <summary>
        /// 插件初始化时，自定义操作逻辑，此处应该将插件程序集添加到ApplicationPartManager或BuildManager中
        /// </summary>
        /// <value>The on initialize plugin.</value>
        public Action<IReadOnlyList<Assembly>> OnInitializePlugin { get; set; }

        public ApplicationStartup(IIocManager iocManager)
        {
            IocManager = iocManager;
        }

        public void Initialize()
        {
            InitializePlugin();
            RegisterBuilders();
        }

        public void InitializePlugin()
        {
            PluginManager.Instance.Initialize();
            var assemblies = PluginManager.Instance.GetManualReferencedAssembly();

            if (OnInitializePlugin == null) throw new Exception("请先订阅 `OnInitializePlugin` ");

            OnInitializePlugin(assemblies);
            InitAssemblyResolver();
        }

        public void RegisterBuilders()
        {
            var builderTypes = AppDomainTypeFinder.Instance.OfType<IIocBuilder>();

            new AutofacBuilderBase(IocManager).Build();
        }

        /// <summary>
        /// Init the assembly resolver <see cref="AppDomain.AssemblyResolve"/>.
        /// </summary>
        private void InitAssemblyResolver()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (object sender, ResolveEventArgs args) =>
            {
                var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName == args.Name);
                if (assembly != null)
                    return assembly;

                assembly = IocManager.Resolve<ITypeFinder>().GetAssemblies().FirstOrDefault(a => a.FullName == args.Name);
                return assembly;
            };
        }
    }
}
