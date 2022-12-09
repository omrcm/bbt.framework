using data.test.app.Database;
using FluentScheduler;
using outbox.test.app.Outbox;
using outbox.test.app.Outbox.Consumer;

namespace outbox.test.app
{
    public class ProducerWorker : BackgroundService
    {
        private readonly ILogger<ProducerWorker> _logger;
        private readonly TestOutboxProducer testOutboxProducer;
        private readonly ITestRepository testRepository;
        private readonly TestOutboxWorker testOutboxWorker;

        public ProducerWorker(
            ILogger<ProducerWorker> logger,
            TestOutboxProducer _testOutboxProducer, 
            ITestRepository _testRepository,
            TestOutboxWorker _testOutboxWorker)
        {
            _logger = logger;

            testRepository = _testRepository;
            testOutboxProducer = _testOutboxProducer;
            testOutboxWorker = _testOutboxWorker;


            JobManager.Initialize();
        
            JobManager.AddJob(
                async () => await testOutboxWorker.Process(),
                s => s.ToRunNow().AndEvery(10).Seconds()
            );

            JobManager.Start();

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await testRepository.EnsureCreated();



            for (int i = 1; i < 100; i++)
            {
                try
                {
                    TestOutboxModel model = new TestOutboxModel();
                    model.Data = new TestModel();
                    // model.Data.Title = $"Title {i}";
                    model.Data.NAME = $"NAME {i}";
                    model.TimeBetweenRetrySeconds = 10;
                    model.MaxRetry = 10;


                    await testOutboxProducer.Send(model);

                    Task.Delay(1 * 1000);
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}