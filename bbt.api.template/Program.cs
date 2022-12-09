
using bbt.api.template;
using bbt.framework.common.Extensions;
using Elastic.Apm.NetCoreAll;

string enviromentName = "ASPNETCORE_ENVIRONMENT";

IHost host = Host.CreateDefaultBuilder(args)
    .UseConsulSettings(typeof(Program), enviromentName)
    .UseSeriLog("entegrasyon", enviromentName)
   .ConfigureWebHostDefaults(webBuilder =>
   {
       webBuilder.UseStartup<Startup>();
   })
   .Build();



await host.RunAsync();
