using bbt.framework.common.Extensions;
using bbt.worker.template;
using Elastic.Apm.NetCoreAll;

string enviromentName = "DOTNET_ENVIRONMENT";

IHost host = Host.CreateDefaultBuilder(args)
    .UseConsulSettings(typeof(Program), enviromentName)
    .UseSeriLog("entegrasyon", enviromentName)
    .ConfigureServices(services =>
    {
        services.AddHostedService<SampleWorker>();
    })
    .UseAllElasticApm()
    .Build();

await host.RunAsync();
