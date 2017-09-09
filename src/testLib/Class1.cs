using Naruto.Dependency.Installers;
using System;
using Naruto.Dependency.Abstraction;
using Naruto.Reflection;

namespace testLib
{
    public class Class1 : IDependencyInstaller
    {
        public int Order => 10;

        public void Install(IIocManager manager, ITypeFinder finder)
        {
            manager.Register<string>(LifetimeStyle.Singleton);
        }
    }
}
