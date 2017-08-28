using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Naruto.Plugins
{
    public class PluginFinder : IPluginFinder
    {
        private IList<PluginDescriptor> _plugins;
        private bool _arePluginsLoaded;

        protected virtual void EnsurePluginsAreLoaded()
        {
            if (!_arePluginsLoaded)
            {
                var foundPlugins = PluginManager.Instance.ReferencedPlugins.ToList();
                _plugins = foundPlugins.OrderBy(f => f.DisplayOrder).ToList();
                _arePluginsLoaded = true;
            }
        }

        protected virtual bool CheckLoadMode(PluginDescriptor pluginDescriptor, LoadPluginsMode loadMode)
        {
            if (pluginDescriptor == null)
                throw new ArgumentNullException(nameof(pluginDescriptor));

            switch (loadMode)
            {
                case LoadPluginsMode.All:
                    //no filering
                    return true;
                case LoadPluginsMode.Installed:
                    return pluginDescriptor.Installed;
                case LoadPluginsMode.NotInstalled:
                    return !pluginDescriptor.Installed;
                default:
                    throw new Exception("Not supported LoadPluginsMode");
            }
        }

        public virtual IEnumerable<PluginDescriptor> GetPluginDescriptors<T>(LoadPluginsMode loadMode) where T : class, IPlugin
        {
            return GetPluginDescriptors(loadMode)
                        .Where(p => typeof(T).IsAssignableFrom(p.PluginType));
        }

        public virtual IEnumerable<PluginDescriptor> GetPluginDescriptors(LoadPluginsMode loadMode)
        {
            //ensure plugins are loaded
            EnsurePluginsAreLoaded();

            return _plugins.Where(p => CheckLoadMode(p, loadMode));
        }

        public virtual PluginDescriptor GetPluginDescriptorBySystemName(string systemName, LoadPluginsMode loadMode = LoadPluginsMode.Installed)
        {
            return GetPluginDescriptors(loadMode)
                        .SingleOrDefault(p => p.SystemName.Equals(systemName, StringComparison.InvariantCultureIgnoreCase));
        }

        public virtual PluginDescriptor GetPluginDescriptorBySystemName<T>(string systemName, LoadPluginsMode loadMode = LoadPluginsMode.Installed)
            where T : class, IPlugin
        {
            return GetPluginDescriptors<T>(loadMode)
                        .SingleOrDefault(p => p.SystemName.Equals(systemName, StringComparison.InvariantCultureIgnoreCase));
        }

        public virtual void ReloadPlugins()
        {
            _arePluginsLoaded = false;
            EnsurePluginsAreLoaded();
        }
    }
}
