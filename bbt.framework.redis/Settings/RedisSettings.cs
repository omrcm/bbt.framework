using System.Collections.Generic;

namespace bbt.framework.redis
{
    public class RedisModel
    {
        public string Host { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }

    }

    public class RedisSettings
    {
        public RedisSettings()
        {
            RedisModelList = new List<RedisModel>();
        }
        public List<RedisModel> RedisModelList { get; set; }
    }
}
