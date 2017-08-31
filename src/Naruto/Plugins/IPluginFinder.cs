using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Naruto.Plugins
{
    public interface IPluginFinder
    {
        IEnumerable<PluginDescriptor> GetPluginDescriptors<T>(LoadPluginsMode loadMode) where T : class, IPlugin;

        IEnumerable<PluginDescriptor> GetPluginDescriptors(LoadPluginsMode loadMode);

        PluginDescriptor GetPluginDescriptorBySystemName(string systemName, LoadPluginsMode loadMode = LoadPluginsMode.Installed);

        PluginDescriptor GetPluginDescriptorBySystemName<T>(string systemName, LoadPluginsMode loadMode = LoadPluginsMode.Installed)
            where T : class, IPlugin;

		PluginDescriptor GetPluginDescriptorByAssembly(Assembly assembly);

        void ReloadPlugins();
    }
}
