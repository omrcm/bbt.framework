using RestEase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bbt.framework.dengage.Business
{
    public interface IDengageSMS
    {
        [Post("/transactional/sms")]
        [Header("Authorization", "Bearer")]
        Task<DengageSMSResponseModel> TransactionalSMS([Body(BodySerializationMethod.Serialized)] DengageSMSRequestModel model);
    }
}
