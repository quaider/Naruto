
namespace Naruto.Runtime.Configuration.Redis
{
    public class RedisOptions
    {
        public string ConnectionString { get; set; }

        public RedisCacheOptions CacheOptions { get; set; }
    }
}
