using bbt.framework.data;
using data.test.app;
using data.test.app.Database;
using Microsoft.EntityFrameworkCore;

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
        // Yugabyte
        // services.AddDbContext<TestYugaDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        // services.AddSingleton<ITestRepository>(x => new TestYugaRepository(x.CreateScope().ServiceProvider.GetRequiredService<TestYugaDbContext>()));

        // MongoDB
        services.AddSingleton(x => new TestMongoDbContext(configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>()));
        services.AddSingleton<ITestRepository>(x => new TestMongoRepository(x.GetRequiredService<TestMongoDbContext>()));


       services.AddHostedService<ReaderWorker>();
        // services.AddHostedService<WriterWorker>();
    })
    .Build();

await host.RunAsync();
