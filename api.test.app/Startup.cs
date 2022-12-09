using bbt.framework.api;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace api.test.app
{
    public class Startup : BaseStartup
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
        }

        public override void CustomConfigure(IApplicationBuilder app)
        {
            //  throw new NotImplementedException();
        }

        public override void CustomConfigureServices(IServiceCollection services)
        {
            //  throw new NotImplementedException();
        }

        public override void CustomSignalRHub(IApplicationBuilder app)
        {
            // throw new NotImplementedException();
        }
    }
}
