using System;
namespace Naruto.Plugins
{
    public abstract class BasePlugin : IPlugin
    {
        public BasePlugin()
        {
            
        }

        public abstract PluginDescriptor PluginDescriptor { get; set; }

        public void Install()
        {

        }

        public void Uninstall()
        {

        }
    }
}
