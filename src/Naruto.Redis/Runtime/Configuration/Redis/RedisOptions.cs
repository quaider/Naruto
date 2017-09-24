
namespace Naruto.Runtime.Configuration.Redis
{
    public class RedisOptions
    {
        public string ConnectionString { get; set; }

        public int DefaultDatabaseId { get; set; } = -1;

        public bool EnableCache { get; set; }

        public RedisCacheOptions CacheOptions { get; set; }

        public void UseRedisCache()
        {
            EnableCache = true;
        }
    }
}
