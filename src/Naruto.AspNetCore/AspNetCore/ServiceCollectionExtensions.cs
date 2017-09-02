using System;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Naruto.AspNetCore.Razor;

namespace Naruto.AspNetCore
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPluginViewLocations(this IServiceCollection services)
        {
            services.Configure<RazorViewEngineOptions>(opt =>
            {
                //opt.ViewLocationExpanders.Add();
                opt.ViewLocationExpanders.Add((new PluginViewLocationExpander()));
            });

            //IRouteProvider a;

            return services;
        }
    }
}
