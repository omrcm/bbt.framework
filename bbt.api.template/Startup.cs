using bbt.api.template.Default;
using Elastic.Apm.NetCoreAll;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace bbt.api.template
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks();

            services.AddControllers()
                    .AddNewtonsoftJson(opts => {
                        opts.SerializerSettings.Converters.Add(new StringEnumConverter());
                        opts.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    });

            JsonSerializerSettings settings = new();
            settings.Converters.Add(new StringEnumConverter());
            JsonConvert.DefaultSettings = () => settings;

            services.AddApiVersioning(v =>
            {
                v.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                v.AssumeDefaultVersionWhenUnspecified = true;
            });

            services.AddVersionedApiExplorer(setup =>
            {
                setup.GroupNameFormat = "'v'VVV";
                setup.SubstituteApiVersionInUrl = true;
            });

            services.AddSwaggerGen(c =>
            {
                //c.SwaggerDoc("v1", new OpenApiInfo { Title = "bbt.gateway.messaging", Version = "v1" });
                c.EnableAnnotations();
                //TODO: is process info came from header or body ? Decide
                //c.OperationFilter<AddRequiredHeaderParameter>();
            });
            services.AddSwaggerGenNewtonsoftSupport();
            services.ConfigureOptions<ConfigureSwaggerOptions>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            
            app.UseDeveloperExceptionPage();
            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint(
                        $"/swagger/{description.GroupName}/swagger.json",
                        description.GroupName.ToUpperInvariant());
                    options.RoutePrefix = "";
                }
            });


            app.UseHealthChecks("/hc", new HealthCheckOptions()
            {
                ResultStatusCodes =
            {
                [Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Healthy] = StatusCodes.Status200OK,
                [Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Degraded] = StatusCodes.Status503ServiceUnavailable,
                [Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
            }
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseAllElasticApm(Configuration);

        }
    }
}
