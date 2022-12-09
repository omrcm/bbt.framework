using bbt.framework.dengage;
using RestEase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kafka.ksqldb.test.app.Business.NewFolder
{
    public class KsqlResponse
    {
        public string error_code { get; set; }
        public string message { get; set; }

    }
    public class KsqlRequest
    {
        public string ksql { get; set; }
        public Dictionary<string,string> streamsProperties { get; set; }

        public KsqlRequest()
        {
            streamsProperties = new Dictionary<string, string>();
        }
    }


    public class BKsql : BaseRefit<IKsql>
    {
        public BKsql(ILogger logger) : base("http://localhost:8088", string.Empty, logger)
        {

        }
        public override string controllerName => "Ksql";

        public async Task<object> ksql(KsqlRequest request)
        {
            return await ExecutePolly(() =>
            {
                return api.ksql(request).Result;
            }
            );
        }


    }

    public interface IKsql
    {
        [Post("/ksql")]
        Task<object> ksql([Body(BodySerializationMethod.Serialized)] KsqlRequest model);

        [Post("/query")]
        Task<object> query([Body(BodySerializationMethod.Serialized)] KsqlRequest model);
    }
}
