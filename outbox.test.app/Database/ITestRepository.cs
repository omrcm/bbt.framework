using bbt.framework.data;
using outbox.test.app.Outbox;

namespace data.test.app.Database
{
    public interface ITestRepository : IBaseRepository<TestOutboxModel>
    {
    }
}
