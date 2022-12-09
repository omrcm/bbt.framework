using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Collections.Generic;

namespace bbt.framework.redis
{
    public class BBTRedisConnection
    {
        public ConnectionMultiplexer connectionMultiplexer { get; set; }

        RedisSettings redisSettings;
        public BBTRedisConnection(RedisSettings _redisSettings)
        {
            redisSettings = _redisSettings;
            ConfigurationOptions configurationOptions = GetConfig();
            CreateConnection(configurationOptions);
        }

        private void CreateConnection(ConfigurationOptions configurationOptions)
        {
            try
            {
                connectionMultiplexer = ConnectionMultiplexer.Connect(configurationOptions);
            }
            catch (Exception e)
            {
            }
        }

        private ConfigurationOptions GetConfig()
        {
            ConfigurationOptions configurationOptions = new ConfigurationOptions();
            //configurationOptions.SyncTimeout = 60 * 1000;
            //configurationOptions.AbortOnConnectFail = false;
            //configurationOptions.Ssl = false;
            //// configurationOptions.CheckCertificateRevocation = false;
            //configurationOptions.ConnectTimeout = 60 * 1000;
            configurationOptions.Proxy = Proxy.Twemproxy;

            configurationOptions.Password = redisSettings.RedisModelList[0].Password;

            List<RedisModel> redisModelList = redisSettings.RedisModelList;

            foreach (RedisModel redisModel in redisModelList)
            {
                configurationOptions.EndPoints.Add(redisModel.Host, redisModel.Port);
            }
            return configurationOptions;
        }
    }
}
