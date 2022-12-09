using bbt.framework.api.Model;
using bbt.framework.api.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace bbt.framework.api
{
    public abstract class BaseStartup
    {
        public abstract void CustomConfigureServices(IServiceCollection services);
        public abstract void CustomConfigure(IApplicationBuilder app);
        public abstract void CustomSignalRHub(IApplicationBuilder app);

        public IConfiguration Configuration { get; }

        public BaseStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }



        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            AddDependecyFromSettings(services);

            services.AddControllers();

            #region api version
            services.AddApiVersioning(setup =>
            {
                setup.DefaultApiVersion = new ApiVersion(1, 0);
                setup.AssumeDefaultVersionWhenUnspecified = true;
                setup.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(setup =>
            {
                setup.GroupNameFormat = "'v'VVV";
                setup.SubstituteApiVersionInUrl = true;
            });

            services.AddSwaggerGen();

            services.ConfigureOptions<ConfigureSwaggerOptions>();

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
    IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.DefaultModelsExpandDepth(-1);

                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint(
                        $"{description.GroupName}/swagger.json",
                        description.GroupName.ToUpperInvariant());
                }
            });


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        private void AddDependecyFromSettings(IServiceCollection services)
        {
            // configden gelen dependencyler ekleniyor.
            DependencySettings dependencySettings = Configuration.Get<DependencySettings>();
            if (
                dependencySettings != null &&
                dependencySettings.DependencyModelList != null &&
                dependencySettings.DependencyModelList.Count > 0)
            {
                foreach (DependencyModel service in dependencySettings.DependencyModelList)
                {
                    services.Add(new ServiceDescriptor(serviceType: Type.GetType(service.ServiceType),
                                                       implementationType: Type.GetType(service.ImplementationType),
                                                       lifetime: service.Lifetime));
                }
            }
        }
    }
}
