using data.test.app.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace data.test.app
{
    public class WriterWorker : BackgroundService
    {
        private readonly ILogger<WriterWorker> logger;
        private readonly ITestRepository testRepository;
        public WriterWorker(ILogger<WriterWorker> _logger, ITestRepository _testRepository)
        {
            logger = _logger;
            testRepository = _testRepository;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await testRepository.EnsureCreated();

                TestModel model;
                for (int i=1;i<100;i++)
                {
                    model = new() { Title = $"Title {i}" };
                    await testRepository.Insert(model);

                }
                await testRepository.Save();
            }
            catch (Exception ex)
            {

            }

            Console.ReadLine();
        }
    }
}