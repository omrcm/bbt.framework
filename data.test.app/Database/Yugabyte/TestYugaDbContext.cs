using bbt.framework.data;
using Microsoft.EntityFrameworkCore;

namespace data.test.app.Database
{

    public class TestYugaDbContext : DbContext, IDbContext
    {
        public TestYugaDbContext(DbContextOptions<TestYugaDbContext> options) : base(options)
        {

        }
        public virtual DbSet<TestModel> TestModels { get; set; }
    }

}
