using Microsoft.Extensions.DependencyInjection;

namespace bbt.framework.redis
{
    public static class RedisExtension
    {
        public static void AddMiddtRedisCache(this IServiceCollection services, RedisSettings redisSettings)
        {
            //services.AddSingleton(new BBTRedisConnection(redisSettings));
            services.AddSingleton<BBTRedislock>();
            //services.AddSingleton<BBTRedisCache>();
        }

    }
}
