using bbt.framework.data;

namespace data.test.app.Database
{
    public class TestMongoRepository : BaseMongoDBRepository<TestModel, TestMongoDbContext>, ITestRepository
    {
        public TestMongoRepository(TestMongoDbContext context) : base(context)
        {
        }
    }
}
