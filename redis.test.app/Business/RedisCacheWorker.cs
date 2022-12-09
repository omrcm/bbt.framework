using bbt.framework.redis;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace redis.test.app.Business
{
    public class RedisCacheWorker : BackgroundService
    {
        private readonly ILogger<BackgroundService> _logger;
        private readonly BBTRedisCache bBTRedisCache;

        public RedisCacheWorker(ILogger<BackgroundService> logger, BBTRedisCache _bBTRedisCache)
        {
            _logger = logger;

            bBTRedisCache = _bBTRedisCache;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Parallel.For(5, 30, new ParallelOptions() { MaxDegreeOfParallelism = 20 }, async ctr =>
            {
                int? result = await bBTRedisCache.ReadWriteWithLock<int?>(async () =>
                {
                    Console.WriteLine($"Lock Value: {ctr}");

                    await Task.Delay(1 * 1000);

                    return ctr;

                }, "Resource_2", 10, 10);


                Console.WriteLine($"Cache Value: {result}");
            });
        }
    }
}