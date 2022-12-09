using bbt.framework.kafka;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace kafka.test.app
{
    // public class DataModelConsumer : BaseConsumer<KafkaModel<DataModel>>
    public class DataModelConsumer : BaseConsumer<DataModel>
    {
        public DataModelConsumer(
            KafkaSettings _kafkaSettings,
            CancellationToken _cancellationToken,
            ILogger _logger) : base(_kafkaSettings, _cancellationToken, _logger)
        {

        }
        // public override async Task<bool> Process(KafkaModel<DataModel> model)
        public override async Task<bool> Process(DataModel model)
        {
            // Console.WriteLine($"Data Received - {model.message.data.Name}");
            //JObject obj = JObject.Parse(model);


            //try
            //{
            //   var id = Convert.ToInt64(obj["message"]["data"]["Id"].ToString());
            //}
            //catch (Exception ex)
            //{

            //}
            return true;
        }
    }
}

