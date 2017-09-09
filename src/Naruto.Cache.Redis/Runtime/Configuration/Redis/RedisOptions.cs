
namespace Naruto.Runtime.Configuration.Redis
{
    public class RedisOptions
    {
        public string ConnectionString { get; set; }

        public RedisCacheOptions RedisCacheOptions { get; set; }
    }
}
