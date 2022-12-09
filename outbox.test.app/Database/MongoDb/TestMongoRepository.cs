using bbt.framework.data;
using outbox.test.app.Outbox;

namespace data.test.app.Database
{
    public class TestMongoRepository : BaseMongoDBRepository<TestOutboxModel, TestMongoDbContext>, ITestRepository
    {
        public TestMongoRepository(TestMongoDbContext context) : base(context)
        {
        }
    }
}
