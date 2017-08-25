using Naruto.Reflection;
using Naruto.Dependency.Abstraction;

namespace Naruto.Dependency.Installers
{
    public interface IDependencyInstaller
    {
        void Install(IIocManager manager, ITypeFinder finder);

        int Order { get; }
    }
}
