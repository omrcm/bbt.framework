using bbt.framework.outbox;
using bbt.framework.redis;
using data.test.app.Database;
using Microsoft.EntityFrameworkCore;

namespace outbox.test.app.Outbox.Consumer
{

    public class TestOutboxWorker : BaseOutboxWorker<TestModel, TestOutboxModel>
    {
        public TestOutboxWorker(ITestRepository repository, TestOutboxProducer producer) : base(repository, producer)
        {

        }

        public override async Task<List<TestOutboxModel>> GetData()
        {
            List<TestOutboxModel> testList = baseRepository.GetAll().Include(x => x.Data).ToList();
            return testList;

        }
    }
}
