using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bbt.framework.dengage.Business
{
    public class BDengageSMS : BaseRefit<IDengageSMS>
    {
        public BDengageSMS(ILogger logger,string token):base("", token, logger)
        {

        }
        public override string controllerName => "transactional";

        public async Task<DengageSMSResponseModel> TransactionalSMS(DengageSMSRequestModel request)
        {
            return await ExecutePolly(() =>
            {
                return api.TransactionalSMS(request).Result;
            }
            );
        }

        //public async Task<DengageSMSResponseModel> GetTransactionalSmsReport(DengageSMSRequestModel request)
        //{
        //    return await ExecutePolly(() =>
        //    {
        //        return api.TransactionalSMS(request).Result;
        //    }
        //    );
        //}
    }
}
