using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Razor;
using Naruto.Dependency;
using Naruto.Plugins;

namespace Naruto.AspNetCore.Razor
{
    /// <summary>
    /// determine the paths to search the view
    /// </summary>
    public class PluginViewLocationExpander : IViewLocationExpander
    {
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            return null;
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            //var pluginFinder = IocManager.Instance.Resolve<IPluginFinder>();
            //var descriptor = pluginFinder.GetPluginDescriptorByAssembly(context.ActionContext.GetType().Assembly);
            //var areaName = descriptor.OrderBy
        }
    }
}
