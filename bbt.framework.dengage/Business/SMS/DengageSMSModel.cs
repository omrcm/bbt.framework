using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bbt.framework.dengage
{

    public class DengageSmsMessageModel
    {
        public string smsFromId { get; set; }
        public string message { get; set; }
    }
    public class DengageSmsSendModel
    {
        public string to { get; set; }
    }
    public class DengageSMSRequestModel
    {
        public DengageSmsMessageModel content { get; set; }
        public DengageSmsSendModel send { get; set; }
        public List<string> tags { get; set; }
    }



    public class DengageSMSResponseModel
    {
        public string transactionId { get; set; }
        public string code { get; set; }
        public string message { get; set; }

    }



}
