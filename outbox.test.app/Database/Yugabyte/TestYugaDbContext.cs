using bbt.framework.data;
using Microsoft.EntityFrameworkCore;
using outbox.test.app.Outbox;

namespace data.test.app.Database
{

    public class TestYugaDbContext : DbContext, IDbContext
    {
        public TestYugaDbContext(DbContextOptions<TestYugaDbContext> options) : base(options)
        {

        }
        public virtual DbSet<TestModel> TestModels { get; set; }
        public virtual DbSet<TestOutboxModel> TestOutboxModels { get; set; }
    }

}
