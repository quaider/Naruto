using Naruto.Reflection;
using Naruto.Dependency.Abstraction;

namespace Naruto.Dependency.Installers
{
    /// <summary>
    /// 安装或注册依赖到ioc容器中
    /// 注：插件中也许并不需要实现，因为插件中会共享主站依赖
    /// </summary>
    public interface IDependencyInstaller
    {
        void Install(IIocManager manager, ITypeFinder finder);

        int Order { get; }
    }
}
