using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Naruto.AspNetCore;
using Naruto.Dependency;
using Naruto.Configuration.Startup;
using Naruto.Reflection;
using Naruto.Plugins;
using Naruto.Reflection.Extensions;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace netcore_demo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            return services.AddNaruto(options => { });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            var types = AppDomainTypeFinder.Instance.OfType<IPlugin>();
            foreach (var t in types)
            {
                var ins = t.CreateInstance<IPlugin>();
                //ins.Install();
            }

            //ApplicationPartManager: http://www.cnblogs.com/Leo_wl/p/6078434.html

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });


        }
    }
}
