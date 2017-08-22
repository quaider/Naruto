using Naruto.Reflection;
using System;
using System.Linq;
using System.Reflection;

namespace Naruto.Dependency.Abstraction
{
    /// <summary>
    /// 依赖注入构建器基类
    /// </summary>
    public abstract class IocBuilderBase : IIocBuilder
    {
        private bool _isBuilded;
        private IIocManager _iocManager;
        protected ITypeFinder Finder { get; private set; }

        protected IocBuilderBase(IIocManager iocManager)
        {
            _iocManager = iocManager;
            Finder = new AppDomainTypeFinder();
        }

        public IServiceProvider ServiceProvider { get; private set; }

        public IServiceProvider Build()
        {
            if (_isBuilded)
            {
                return ServiceProvider;
            }

            RegisterCustomTypes(_iocManager);
            ServiceProvider = Build(_iocManager, GetAssemblies());
            _isBuilded = true;

            return ServiceProvider;
        }

        /// <summary>
        /// 添加自定义服务映射
        /// </summary>
        protected virtual void RegisterCustomTypes(IIocManager iocManager)
        {

        }

        protected virtual Assembly[] GetAssemblies()
        {
            return Finder.GetAssemblies().ToArray();
        }

        protected abstract IServiceProvider Build(IIocManager iocManager, params Assembly[] assemblies);
    }
}
