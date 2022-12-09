using bbt.framework.data;
using data.test.app.Database;

namespace data.test.app
{
    public class ReaderWorker : BackgroundService
    {
        private readonly ILogger<ReaderWorker> logger;
        private readonly ITestRepository testRepository;
        public ReaderWorker(ILogger<ReaderWorker> _logger, ITestRepository _testRepository)
        {
            logger = _logger;
            testRepository = _testRepository;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {         
            try
            {
                await testRepository.EnsureCreated();

                List<TestModel>? listTestModel = testRepository.GetAll().OrderBy(x => x.Id)?.ToList();
                foreach (TestModel model in listTestModel)
                {
                    Console.WriteLine($" Id : {model.Id} Title: {model.Title}");
                }
            }
            catch (Exception ex)
            {

            }

            Console.ReadLine();
        }
    }
}