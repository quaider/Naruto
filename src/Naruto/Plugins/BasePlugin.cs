using System;
namespace Naruto.Plugins
{
    public abstract class BasePlugin : IPlugin
    {
        public BasePlugin()
        {

        }

        public virtual PluginDescriptor PluginDescriptor { get; set; }

        public virtual void Install()
        {
            PluginManager.MarkPluginAsInstalled(PluginDescriptor.SystemName);
        }

        public virtual void Uninstall()
        {
            PluginManager.MarkPluginAsUninstalled(PluginDescriptor.SystemName);
        }
    }
}
