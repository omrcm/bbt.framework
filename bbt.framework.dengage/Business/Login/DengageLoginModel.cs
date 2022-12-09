using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bbt.framework.dengage
{
    public record DengageLoginRequestModel
    {
        public string userkey { get; set; }
        public string password { get; set; }
    }
    public record DengageLoginResponseModel
    {
        public string access_token { get; set; }

        public int expires_in { get; set; }
    }
}
