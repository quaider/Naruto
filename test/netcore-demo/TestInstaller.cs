using Naruto.Dependency.Installers;
using Naruto.Dependency.Abstraction;
using Naruto.Reflection;
using Naruto.Runtime.Caching;
using Naruto.Runtime.Caching.Redis;
using Naruto.Dependency;
using Naruto.Runtime.Configuration.Redis;

namespace netcore_demo
{
    public class TestInstaller : IDependencyInstaller
    {
        public int Order => -100;

        public void Install(IIocManager ioc, ITypeFinder finder)
        {
            
        }
    }
}
