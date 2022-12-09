using bbt.framework.redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using redis.test.app.Business;

var builder = new ConfigurationBuilder();
builder.SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{GetEnviroment()}.json", true, true)
        .AddEnvironmentVariables();

//only add secrets in development
#if DEBUG
builder.AddUserSecrets<Program>();
#endif


IConfigurationRoot configuration = builder.Build();

string? GetEnviroment()
{
    return Environment.GetEnvironmentVariable("ENVIRONMENT");
}


IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddLogging(b =>
        {
            b.AddConsole();
            b.SetMinimumLevel(LogLevel.Information);
        });

        var serviceProvider = services.BuildServiceProvider();
        var logger = serviceProvider.GetService<ILogger<BackgroundService>>();
        services.AddSingleton(typeof(ILogger), logger);

        //services.AddSingleton(x => new BBTRedisConnection(configuration.GetSection(nameof(RedisSettings)).Get<RedisSettings>()));
        //services.AddTransient(x => new BBTRedislock(x.GetRequiredService<BBTRedisConnection>(), x.GetRequiredService<ILogger>()));
        //services.AddTransient(x => new BBTRedisCache(x.GetRequiredService<BBTRedisConnection>(), x.GetRequiredService<BBTRedislock>()));



        // services.AddHostedService<RedisLockWorker>();
        services.AddHostedService<RedisCacheWorker>();
    })
    .Build();

await host.RunAsync();

