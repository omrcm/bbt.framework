namespace outbox.test.app.Outbox.Consumer
{
    public class ConsumerWorker : BackgroundService
    {
        private readonly ILogger<ProducerWorker> _logger;
        private readonly TestOutboxWorker testOutboxWorker;

        public ConsumerWorker(ILogger<ProducerWorker> logger, TestOutboxWorker _testOutboxWorker)
        {
            _logger = logger;

            testOutboxWorker = _testOutboxWorker;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await testOutboxWorker.Process();
        }
    }
}