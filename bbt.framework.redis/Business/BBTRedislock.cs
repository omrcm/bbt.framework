using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace bbt.framework.redis
{
    public class BBTRedislock
    {
        private BBTRedisConnection baseRedisConnection;
        private ILogger baseLog;
        public BBTRedislock(BBTRedisConnection _baseRedisConnection, ILogger _baseLog)
        {
            baseLog = _baseLog;
            baseRedisConnection = _baseRedisConnection;
        }

        public virtual async Task<T> ExecuteMethod<T>(Func<Task<T>> action, string resource, TimeSpan expiryTime)
        {
            T result = default(T);
            try
            {
                var db = baseRedisConnection.connectionMultiplexer.GetDatabase();
                RedisValue token = (RedisValue)Guid.NewGuid().ToString();
                try
                {
                    if (await db.LockTakeAsync(resource, token, expiryTime))
                    {
                        try
                        {
                            result = await Task.Run<T>(action);
                        }
                        catch (Exception ex)
                        {
                            baseLog.LogError("RedLockHelper - ExecuteMethod  - 1.1  " + ex.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    baseLog.LogError("RedLockHelper - ExecuteMethod  - 1.3  " + ex.ToString());
                }
                finally
                {
                    db.LockRelease(resource, token);
                }
            }
            catch (Exception ex)
            {
                baseLog.LogError("RedLockHelper - ExecuteMethod - 1.2 " + ex.ToString());
            }

            return result;
        }
        public virtual async Task ExecuteMethod(Func<Task> action, string resource, TimeSpan expiryTime)
        {
            try
            {
                var db = baseRedisConnection.connectionMultiplexer.GetDatabase();
                RedisValue token = (RedisValue)Guid.NewGuid().ToString();
                try
                {
                    if (await db.LockTakeAsync(resource, token, expiryTime))
                    {
                        try
                        {
                            await Task.Run(action);
                        }
                        catch (Exception ex)
                        {
                            baseLog.LogError("RedLockHelper - ExecuteMethod  - 2.1  " + ex.ToString());
                        }

                    }
                }
                catch(Exception ex)
                {
                    baseLog.LogError("RedLockHelper - ExecuteMethod  - 2.3  " + ex.ToString());
                }
                finally
                {
                    db.LockRelease(resource, token);
                }
            }
            catch (Exception ex)
            {
                baseLog.LogError("RedLockHelper - ExecuteMethod  - 2.2  " + ex.ToString());
            }
        }
    }
}
