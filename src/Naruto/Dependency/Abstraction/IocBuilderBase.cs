using Naruto.Reflection;
using System.Linq;
using System.Reflection;

namespace Naruto.Dependency.Abstraction
{
    /// <summary>
    /// 依赖注入构建器基类
    /// </summary>
    internal abstract class IocBuilderBase : IIocBuilder
    {
        private IIocManager _iocManager;
        protected ITypeFinder Finder { get; private set; }

        protected IocBuilderBase(IIocManager iocManager)
        {
            _iocManager = iocManager;
            Finder = new BinFolderFinder();
        }

        public void Build()
        {
            RegisterCustomTypes(_iocManager);
            Build(_iocManager, GetAssemblies());
        }

        /// <summary>
        /// 添加自定义服务映射
        /// </summary>
        protected abstract void RegisterCustomTypes(IIocManager iocManager);

        protected virtual Assembly[] GetAssemblies()
        {
            return Finder.GetAssemblies().ToArray();
        }

        protected abstract void Build(IIocManager iocManager, params Assembly[] assemblies);
    }
}
