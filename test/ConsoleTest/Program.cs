using Microsoft.Extensions.DependencyInjection;
using Naruto.Dependency;
using Naruto.Redis.Providers;
using Naruto.Runtime.Caching;
using Naruto.Runtime.Caching.Redis;
using System;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();

            var provider = new NarutoServiceProvider(null, services);
            provider.AddRedisOptions(f =>
            {
                f.ConnectionString = "127.0.0.1:6379,allowAdmin=true,syncTimeout=1000000";
                f.DefaultDatabaseId = 6;
            }).Build();

            var cacheManager = IocManager.Instance.Resolve<ICacheManager>();

            var redisService = IocManager.Instance.Resolve<IRedisDatabaseProvider>().GetService();
            redisService.StringSet<int>("fuck", 1, TimeSpan.FromDays(1));
            redisService.StringSet<string>("fuck1", "1", TimeSpan.FromDays(1));

            Console.WriteLine(redisService.StringGetOrDefault<string>("fuck"));
            Console.WriteLine(redisService.StringGetOrDefault<string>("fuck1"));

            Console.ReadKey();
        }
    }
}
