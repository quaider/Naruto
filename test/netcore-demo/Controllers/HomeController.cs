using Microsoft.AspNetCore.Mvc;
using Naruto.Dependency;
using Naruto.Redis;
using Naruto.Redis.Providers;
using Naruto.Runtime.Caching;
using Naruto.Runtime.Caching.Redis;
using System;

namespace netcore_demo.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {

        }

        public IActionResult Index()
        {
            var c = IocManager.Instance.Resolve<ICacheManager>();
            var b1 = IocManager.Instance.Resolve<IRedisDatabaseProvider>();
            var b = IocManager.Instance.Resolve<IRedisCacheDatabaseProvider>();

            var service1 = b1.GetService();
            var service = b.GetService();
            var c1 = c.GetCache(NarutoCacheNames.ApplicationSettings);

            //r.StringSet("aaa", 1, TimeSpan.FromHours(1));

            return View();
        }
    }
}