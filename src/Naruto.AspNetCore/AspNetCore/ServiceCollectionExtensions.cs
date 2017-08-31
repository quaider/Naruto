using System;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;

namespace Naruto.AspNetCore
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPluginViewLocations(this IServiceCollection services)
        {
            services.Configure<RazorViewEngineOptions>(opt =>
            {
                //opt.ViewLocationExpanders.Add();
            });

            //IRouteProvider a;

            return services;
        }
    }
}
