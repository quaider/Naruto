using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Razor;
using Naruto.Dependency;
using Naruto.Plugins;
using Naruto.Collections.Extensions;

namespace Naruto.AspNetCore.Razor
{
    /// <summary>
    /// determine the paths to search the view
    /// </summary>
    public class PluginViewLocationExpander : IViewLocationExpander
    {
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            //{2} is area, {1} is controller, {0} is action
            var pluginFolder = context.Values.GetOrDefault("pluginFolder");
            if (string.IsNullOrWhiteSpace(pluginFolder)) return viewLocations;

            var area = context.Values.GetOrDefault("area");
            var locations = new List<string>();
            if (!string.IsNullOrWhiteSpace(area))
            {
                locations.Add($"/plugins/{pluginFolder}/Areas/{{2}}/Views/{{1}}/{{0}}.cshtml");
                locations.Add($"/plugins/{pluginFolder}/Views/{{2}}/{{1}}/{{0}}.cshtml");
                locations.Add($"/plugins/{pluginFolder}/Areas/{{2}}/Views/Shared/{{0}}.cshtml");
            }

            locations.Add($"/plugins/{pluginFolder}/Views/{{1}}/{{0}}.cshtml");
            locations.Add($"/plugins/{pluginFolder}/Views/Shared/{{0}}.cshtml");

            //Add mvc default locations after ours
            return locations.Union(viewLocations);
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            var pluginFinder = IocManager.Instance.Resolve<IPluginFinder>();
            var descriptor = context.ActionContext.ActionDescriptor as ControllerActionDescriptor;

            //if ControllerActionDescriptor is null, it should be Razor Pages
            //todo: do something with Razor Pages
            if (descriptor == null) return;

            var ass = descriptor.ControllerTypeInfo.Assembly;
            var pluginDescriptor = pluginFinder.GetPluginDescriptorByAssembly(ass);
            if (pluginDescriptor == null) return;

            context.Values["pluginFolder"] = pluginDescriptor.SystemName;
            context.Values["area"] = (context.ActionContext.RouteData.Values["area"] ?? context.AreaName).ToString();
        }
    }
}
