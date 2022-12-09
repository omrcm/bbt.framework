using bbt.framework.kafka;
using kafka.test.app;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

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
         services.AddHostedService<ProducerWorker>();
      //services.AddHostedService<ConsumerWorker>();
        services.AddLogging(b =>
        {
            b.AddConsole();
            b.SetMinimumLevel(LogLevel.Information);
        }
            );

        services.Configure<KafkaSettings>(configuration.GetSection(nameof(KafkaSettings)));

    })
    .Build();

await host.RunAsync();


KafkaSettings kafkaSettings = new KafkaSettings();
kafkaSettings.Topic = new string[] { "test" };
