using bbt.framework.data;

namespace data.test.app.Database
{
    public class TestYugaRepository : BaseEntityFwRepository<TestModel, TestYugaDbContext>, ITestRepository
    {
        public TestYugaRepository(TestYugaDbContext context) : base(context)
        {
        }
    }
}
