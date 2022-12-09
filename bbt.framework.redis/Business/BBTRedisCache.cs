
using bbt.framework.common.Helper;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace bbt.framework.redis
{
    public class BBTRedisCache
    {
        //TimeSpan lockWait = TimeSpan.FromSeconds(10);
        TimeSpan lockRetry = TimeSpan.FromSeconds(1);

        public int DBIndex { get; set; } = 0;


        BBTRedisConnection baseRedisConnection;
        BBTRedislock baseRedislock;

        public BBTRedisCache(BBTRedisConnection _baseRedisConnection, BBTRedislock _baseRedislock)
        {
            baseRedisConnection = _baseRedisConnection;
            baseRedislock = _baseRedislock;
        }

        public async Task<bool> WriteWithLock<T>(Func<Task<T>> RunMethod, string cacheName,  int lockTimeOutSecond, int cacheTimeOutSecond)
        {
            TimeSpan lockTimeout = TimeSpan.FromSeconds(lockTimeOutSecond);
            TimeSpan cacheTimeout = TimeSpan.FromSeconds(cacheTimeOutSecond);

            string lockName = cacheName + "_Lock";


            await baseRedislock.ExecuteMethod(async () =>
            {
                T result = await RunMethod();

                JsonHelper<T> jsonHelper = new JsonHelper<T>();
                string json = jsonHelper.SerializeObject(result);

                IDatabase database = GetDatabase();
                database.StringSetAsync(cacheName, json, cacheTimeout).Wait();

            }, lockName, lockTimeout);


            return true;
        }
        public async Task<T> ReadWriteWithLock<T>(Func<Task<T>> RunMethod, string cacheName,  int lockTimeOutSecond, int cacheTimeOutSecond)
        {
            TimeSpan lockTimeout = TimeSpan.FromSeconds(lockTimeOutSecond);
            TimeSpan cacheTimeout = TimeSpan.FromSeconds(cacheTimeOutSecond);

            string lockName = cacheName + "_Lock";


            T returnModel = await Read<T>(cacheName);

            if (returnModel != null && !returnModel.Equals(default(T)))
                return returnModel;


            returnModel = await baseRedislock.ExecuteMethod<T>(async () =>
            {
                T result = await Read<T>(cacheName);


                if (result != null && !result.Equals(default(T)))
                    return result;

                result = await RunMethod();

                JsonHelper<T> jsonHelper = new JsonHelper<T>();
                string json = jsonHelper.SerializeObject(result);

                IDatabase database = GetDatabase();
                database.StringSetAsync(cacheName, json, cacheTimeout).Wait();

                return result;
            }, lockName, lockTimeout);

            return returnModel;

        }

        public async Task DeleteKey(string cacheName, int lockTimeOutSecond)
        {
            string lockName = cacheName + "_Lock";
            TimeSpan lockTimeout = TimeSpan.FromSeconds(lockTimeOutSecond);
            await baseRedislock.ExecuteMethod(async () =>
            {
                IDatabase database = GetDatabase();
                await database.KeyDeleteAsync(cacheName);

            }, lockName, lockTimeout);

        }

        private T Deserialize<T>(string value)
        {
            T result = default(T);
            if (!string.IsNullOrEmpty(value))
            {
                JsonHelper<T> jsonHelper = new JsonHelper<T>();
                result = jsonHelper.DeserializeObject(value);
            }

            return result;
        }

        public async Task<T> Read<T>(string cacheName)
        {
            IDatabase database = GetDatabase();
            RedisValue result = await database.StringGetAsync(cacheName);
            return Deserialize<T>(result);
        }

        private IDatabase GetDatabase()
        {
            IConnectionMultiplexer redisClient = baseRedisConnection.connectionMultiplexer;
            return redisClient.GetDatabase(DBIndex);
        }
    }
}
