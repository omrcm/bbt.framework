using bbt.framework.data;
using bbt.framework.redis;
using data.test.app.Database;
using Microsoft.EntityFrameworkCore;
using outbox.test.app;
using outbox.test.app.Outbox;
using outbox.test.app.Outbox.Consumer;
using StackExchange.Redis;

ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(
         new ConfigurationOptions
         {
             EndPoints = { "10.180.141.16:32504" },
             Password = "jFhtJyDfLo"
         });

var db = redis.GetDatabase();
var pong = await db.PingAsync();
Console.WriteLine(pong);







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

        // Yugabyte
        //services.AddDbContext<TestYugaDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        //services.AddSingleton<ITestYugaRepository>(x => new TestYugaRepository(x.CreateScope().ServiceProvider.GetRequiredService<TestYugaDbContext>()));

        // MongoDB
        services.AddSingleton(x => new TestMongoDbContext(configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>()));
        services.AddSingleton<ITestRepository>(x => new TestMongoRepository(x.GetRequiredService<TestMongoDbContext>()));


        //services.AddSingleton(x => new BBTRedisConnection(configuration.GetSection(nameof(RedisSettings)).Get<RedisSettings>()));
        //services.AddSingleton(x => new BBTRedislock(x.GetRequiredService<BBTRedisConnection>(), x.GetRequiredService<ILogger>()));
        //services.AddSingleton(x => new BBTRedisCache(x.GetRequiredService<BBTRedisConnection>(), x.GetRequiredService<BBTRedislock>()));
    //    services.AddSingleton<TestOutboxProducer>(
    //x => new TestOutboxProducer(x.GetRequiredService<ITestRepository>()));
    //    services.AddSingleton<TestOutboxWorker>(
    //        x => new TestOutboxWorker(x.GetRequiredService<ITestRepository>(),
    //        x.GetRequiredService<TestOutboxProducer>(),
    //        x.GetRequiredService<BBTRedislock>()));


        services.AddSingleton<TestOutboxProducer>(
            x => new TestOutboxProducer(x.GetRequiredService<ITestRepository>()));
        services.AddSingleton<TestOutboxWorker>(
            x => new TestOutboxWorker(x.GetRequiredService<ITestRepository>(),
            x.GetRequiredService<TestOutboxProducer>()));


        services.AddHostedService<ProducerWorker>();

        // services.AddHostedService<ConsumerWorker>();
    })
    .Build();

await host.RunAsync();
