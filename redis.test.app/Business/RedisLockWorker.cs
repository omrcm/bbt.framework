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
    public class RedisLockWorker : BackgroundService
    {
        private readonly ILogger<BackgroundService> _logger;
        private readonly BBTRedislock bBTRedislock;

        public RedisLockWorker(ILogger<BackgroundService> logger, BBTRedislock _bBTRedislock)
        {
            _logger = logger;

            bBTRedislock = _bBTRedislock;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
           // BBTRedislock bBTRedislock = new BBTRedislock(new BBTRedisConnection(settings), new ConsoleLog());

            Parallel.For(0, 10, new ParallelOptions() { MaxDegreeOfParallelism = 1 }, async ctr =>
            {
                await bBTRedislock.ExecuteMethod(async () =>
                {
                    Console.WriteLine($"Lock Value: {ctr}");

                    await Task.Delay(2 * 1000);

                }, "Resource_1", TimeSpan.FromSeconds(10));

            });
        }
    }
}