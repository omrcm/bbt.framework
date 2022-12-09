using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bbt.framework.dengage.Business
{






    public class BDengageLogin : BaseRefit<IDengageLogin>
    {
        public BDengageLogin(ILogger logger):base("",string.Empty, logger)
        {

        }
        public override string controllerName => "login";

        public async Task<DengageLoginResponseModel> Login(DengageLoginRequestModel request)
        {
            return await ExecutePolly(() =>
            {
                return api.Login(request).Result;
            }
            );
        }


    }
}
