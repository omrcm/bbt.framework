using bbt.framework.kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;

namespace kafka.test.app
{
    public class ConsumerWorker : BackgroundService
    {
        private readonly ILogger<ConsumerWorker> logger;
        private readonly KafkaSettings kafkaSettings;
        public ConsumerWorker(
            ILogger<ConsumerWorker> _logger,
            IOptions<KafkaSettings> _options
            )
        {
            logger = _logger;
            kafkaSettings = _options.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            DataModelConsumer consumer = new DataModelConsumer(kafkaSettings, stoppingToken, logger);
            consumer.ConsumeAsync().Wait();
        }
    }
}