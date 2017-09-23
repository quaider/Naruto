
namespace Naruto.Runtime.Configuration.Redis
{
    public class NarutoRedisOptions
    {
        public NarutoRedisOptions()
        {
            ConnectionString = GetDefaultConnectionString();
            DatabaseId = GetDefaultDatabaseId();
        }

        public string ConnectionString { get; set; }

        public int DatabaseId { get; set; }

        private int GetDefaultDatabaseId()
        {
            //var appSetting = ConfigurationManager.AppSettings[DatabaseIdSettingKey];
            return -1;
        }

        private string GetDefaultConnectionString()
        {
            return "";
        }
    }
}
