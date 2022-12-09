using RestEase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bbt.framework.dengage.Business
{
    public interface IDengageLogin
    {
        [Post("/login")]        
        Task<DengageLoginResponseModel> Login([Body(BodySerializationMethod.Serialized)] DengageLoginRequestModel model);
    }
}
