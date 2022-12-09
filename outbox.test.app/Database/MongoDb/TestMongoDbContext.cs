using bbt.framework.data;
using Microsoft.EntityFrameworkCore;

namespace data.test.app.Database
{
    public class TestMongoDbContext : MongoDbSettings
    {

        public TestMongoDbContext(MongoDbSettings mongoDbSettings)
        {
            DatabaseName = mongoDbSettings.DatabaseName;
            CollectionName = mongoDbSettings.CollectionName;
            ConnectionString = mongoDbSettings.ConnectionString;

        }
    }
}
