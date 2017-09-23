
namespace Naruto.Runtime.Configuration.Redis
{
    public class RedisOptions
    {
        public string ConnectionString { get; set; }

        public int DefaultDatabaseId { get; set; } = -1;

        public RedisCacheOptions CacheOptions { get; set; }
    }
}
