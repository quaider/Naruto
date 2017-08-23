using Naruto.Dependency.Abstraction;
using Naruto.Dependency.Installers;

namespace Naruto
{
    public class ApplicationStartup
    {
        public IIocManager IocManager { get; }

        public ApplicationStartup(IIocManager iocManager)
        {
            IocManager = iocManager;
        }

        public virtual void Initialize()
        {
            RegisterApplicationStartup();

            var installer = new NarutoInstaller();
            installer.Install(IocManager);
        }

        private void RegisterApplicationStartup()
        {
            IocManager.RegisterInstance(this, LifetimeStyle.Singleton);
        }
    }
}
