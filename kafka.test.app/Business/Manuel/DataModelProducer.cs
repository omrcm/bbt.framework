using bbt.framework.kafka;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace kafka.test.app
{
    public class DataModelProducer : BaseProducer<DataModel>
    {
        int index = 0;
        public DataModelProducer(KafkaSettings _kafkaSettings, ILogger _logger) : base(_kafkaSettings, _logger)
        {

        }

        public async Task SendDummy()
        {
            await SendDummy(GenerateDummy());
        }
        public async Task SendDummy(DataModel model)
        {
            await Send(model);
        }

        private DataModel GenerateDummy()
        {
            index++;
            return new DataModel()
            {
                lat = "lat"+index.ToString(),
                lon = "lon" + index.ToString(),
                time = DateTime.UtcNow.AddMinutes(index)
                //Email = Guid.NewGuid().ToString()
            };
        }
    }
}
