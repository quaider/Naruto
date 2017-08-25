using Naruto.Configuration.Startup;
using Naruto.Dependency.Abstraction;
using Naruto.Reflection;
using Naruto.Runtime.Caching.Configuration;

namespace Naruto.Dependency.Installers
{
    internal class NarutoInstaller
    {
        public void Install(IIocManager manager)
        {
            manager.Register<ICachingConfiguration, CachingConfiguration>(LifetimeStyle.Singleton);
            manager.Register<IStartupConfiguration, StartupConfiguration>(LifetimeStyle.Singleton);
        }
    }
}
