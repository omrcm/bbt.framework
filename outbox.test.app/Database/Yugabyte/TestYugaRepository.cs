using bbt.framework.data;
using outbox.test.app.Outbox;

namespace data.test.app.Database
{
    public class TestYugaRepository : BaseEntityFwRepository<TestOutboxModel, TestYugaDbContext>, ITestRepository
    {
        public TestYugaRepository(TestYugaDbContext context) : base(context)
        {
        }
    }
}
