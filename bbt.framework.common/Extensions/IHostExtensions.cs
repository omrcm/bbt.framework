﻿using Consul;
using Elasticsearch.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System;
using Winton.Extensions.Configuration.Consul;


namespace bbt.framework.common.Extensions
{
    public static class IHostExtensions
    {
        /// <summary>
        /// Read Appsettings From Consule<br />
        /// ConsulHost and ConsulToken have to be set in appsettings
        /// </summary>
        /// <param name="host"></param>
        /// <param name="type">Type of Main | usage : typeof(Program)</param>
        /// <returns></returns>
        public static IHostBuilder UseConsulSettings(this IHostBuilder host, Type type,string _environmentName)
        {
            return host.ConfigureAppConfiguration((context, builder) =>
            {
                string applicationName = context.HostingEnvironment.ApplicationName;
                string environmentName = Environment.GetEnvironmentVariable(_environmentName);

                builder.AddJsonFile($"appsettings.{environmentName}.json", false, true)
                .AddUserSecrets(type.Assembly).AddEnvironmentVariables();

                context.Configuration = builder.Build();

                string consulHost = context.Configuration["ConsulHost"];

                void ConsulConfig(ConsulClientConfiguration configuration)
                {
                    configuration.Token = context.Configuration["ConsulToken"];
                    configuration.Address = new Uri(consulHost);
                }

                builder.AddConsul($"{applicationName}/appsettings.json",
                    source =>
                    {
                        source.ReloadOnChange = true;
                        source.ConsulConfigurationOptions = ConsulConfig;
                    });
                builder.AddConsul($"{applicationName}/appsettings.{environmentName}.json",
                    source =>
                    {
                        source.Optional = true;
                        source.ConsulConfigurationOptions = ConsulConfig;
                    });

            });

        }

        /// <summary>
        /// Set Serilog Configuration To Logging Elastic Search<br />
        /// ElasticSearch:ApiKey and ElasticSearch:Url have to be set in appsettings
        /// </summary>
        /// <param name="host"></param>
        /// <param name="indexFormat">Index Format for Elastic Search</param>
        /// <returns></returns>
        public static IHostBuilder UseSeriLog(this IHostBuilder host, string indexFormat, string _environmentName)
        {
            return host.ConfigureAppConfiguration((context, builder) =>
            {
                string environmentName = Environment.GetEnvironmentVariable(_environmentName);
                var configuration = builder.Build();
                Serilog.Debugging.SelfLog.Enable(msg => Console.WriteLine(msg));
                ApiKeyAuthenticationCredentials k = new ApiKeyAuthenticationCredentials(configuration["ElasticSearch:ApiKey"]);
                indexFormat = (environmentName != "Production" ? "nonprod-" : "prod-") + indexFormat;
                Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithEnvironmentName()
                .Enrich.WithMachineName()
                .WriteTo.Console()
                .WriteTo.Debug()
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(configuration["ElasticSearch:Url"]))
                {
                    IndexFormat = indexFormat + "-{0:yyyy-MM}",
                    ModifyConnectionSettings = c => c.ApiKeyAuthentication(k)
                })
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
            }).UseSerilog();

        }
    }
}
