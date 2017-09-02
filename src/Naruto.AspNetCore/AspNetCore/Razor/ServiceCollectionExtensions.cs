using System;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Naruto.AspNetCore.Razor;

namespace Naruto.AspNetCore.Razor
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPluginViewLocations(this IServiceCollection services)
        {
            services.Configure<RazorViewEngineOptions>(opt =>
            {
                opt.ViewLocationExpanders.Add((new PluginViewLocationExpander()));
            });

            return services;
        }
    }
}
