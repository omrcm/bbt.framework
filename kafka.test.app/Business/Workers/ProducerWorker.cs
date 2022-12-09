using bbt.framework.kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace kafka.test.app
{
    public class ProducerWorker : BackgroundService
    {
        private readonly ILogger<ProducerWorker> logger;
        private readonly KafkaSettings kafkaSettings;
        public ProducerWorker(
            ILogger<ProducerWorker> _logger,
            IOptions<KafkaSettings> _options
            )
        {
            logger = _logger;
            kafkaSettings = _options.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            DataModelProducer dataModelProducer = new DataModelProducer(kafkaSettings, logger);


            //string json = @"[
            //{'lat': '56.265595', 'lon': '32.859432', 'id': '2776420839', 'distance': '2.5', 'altitude': '10.2'}]";

            string json = @"[
                {'lat': '56.265595', 'lon': '32.859432', 'id': 1776420839, 'distance': 2.5, 'altitude': 10.2, 'hr': '88',
                 'cadence': '0', 'time': '2019-10-09 21:25:25+00:00', 'rawTime': '0'},
                {'lat': '46.265566', 'lon': '22.859438', 'id': '2776420839', 'distance': '4.6', 'altitude': '10.3', 'hr': '89',
                 'cadence': '79', 'time': '2019-10-09 21:25:27+00:00', 'rawTime': '2'},
                {'lat': '46.265503', 'lon': '22.859488', 'id': '3776420839', 'distance': '12.2', 'altitude': '10.4', 'hr': '92',
                 'cadence': '79', 'time': '2019-10-09 21:25:30+00:00', 'rawTime': '5'},
                {'lat': '46.265451', 'lon': '22.85952', 'id': '4776420839', 'distance': '18.4', 'altitude': '10.4', 'hr': '97',
                 'cadence': '83', 'time': '2019-10-09 21:25:32+00:00', 'rawTime': '7'},
                {'lat': '46.26558', 'lon': '22.85943', 'id': '5776420839', 'distance': '3.1', 'altitude': '10.2', 'hr': '89',
                 'cadence': '79', 'time': '2019-10-09 21:25:26+00:00', 'rawTime': '1'}
            ]";



            List<DataModel> result = JsonConvert.DeserializeObject<List<DataModel>>(json);

            foreach(DataModel model in result)
            {
                dataModelProducer.SendDummy(model).Wait();
            }

            //Parallel.For(0, 1000000, new ParallelOptions() { MaxDegreeOfParallelism = 10 }, ctr =>
            //{
            //    dataModelProducer.SendDummy().Wait();
            //});
        }
    }
}