using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Controllers;
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
            //viewLocations is mvc default view locations
            //{2} is area, {1} is controller,{0} is the action
            string[] locations = new string[] { 
                "/plugins/Plugin.Test/Views/{2}/{1}/{0}.cshtml",
                "/plugins/Plugin.Test/Views/{1}/{0}.cshtml",
            };
            return locations.Union(viewLocations);          //Add mvc default locations after ours

            //return null;
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            var pluginFinder = IocManager.Instance.Resolve<IPluginFinder>();
            var descriptor = context.ActionContext.ActionDescriptor as ControllerActionDescriptor;

			//if ControllerActionDescriptor is null, it should be the new features of pages
            //todo: do something with pages
			if (descriptor == null) return;

            var ass = descriptor.ControllerTypeInfo.Assembly;
            var pluginDescriptor = pluginFinder.GetPluginDescriptorByAssembly(ass);
            if (pluginDescriptor == null) return;

            //Naruto.Plugins.Users
            //var areaName = pluginDescriptor.;

            //context.AreaName
        }
    }
}
