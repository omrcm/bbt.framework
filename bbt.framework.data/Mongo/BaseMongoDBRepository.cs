
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace bbt.framework.data
{
    public class BaseMongoDBRepository<TModel, TDbContext> : IBaseRepository<TModel>

      where TModel : BaseDBModel, new()
        where TDbContext : MongoDbSettings, IDbContext
    {
        private readonly IMongoCollection<TModel> mongoCollection;
        private readonly IMongoDatabase mongoDatabase;

        public BaseMongoDBRepository(TDbContext dbContext)
        {
            var mongoClient = new MongoClient(dbContext.ConnectionString);
            mongoDatabase = mongoClient.GetDatabase(dbContext.DatabaseName);
            mongoCollection = mongoDatabase.GetCollection<TModel>(dbContext.CollectionName);
        }
        public async Task Delete(TModel entity)
        {
            await mongoCollection.DeleteOneAsync(x => x.Id.Equals(entity.Id));
        }

        public async Task Delete(List<TModel> entityList)
        {
            foreach (TModel model in entityList)
                await Delete(model);
        }

        public async Task EnsureCreated()
        {

            //public void Insert(IMongoDatabase database)
            //{
            //    var collection = database.GetCollection<BaseMangoSequence>("sequence");
            //    collection.InsertOne(this);
            //}
        }

        public IQueryable<TModel> FindBy(Expression<Func<TModel, bool>> predicate)
        {
            FilterDefinition<TModel> filter = Builders<TModel>.Filter.Where(predicate);
            return mongoCollection.Find<TModel>(filter).ToEnumerable().AsQueryable();
        }

        public async Task<TModel> FirstOrDefault(Expression<Func<TModel, bool>> predicate)
        {
            return FindBy(predicate).FirstOrDefault();
        }

        public IQueryable<TModel> GetAll()
        {
            return FindBy(x => true);
        }

        public Task<TModel> GetById(int id)
        {
            return FirstOrDefault(x => x.Id.Equals(id));
        }

        public TDbContext GetDbContext()
        {
            //throw new NotImplementedException();
            return default(TDbContext);
        }

        public async Task Insert(TModel entity)
        {
            entity.Id = mongoDatabase.GetNextSequenceValue(entity.GetType().Name);
            await mongoCollection.InsertOneAsync(entity); ;
        }

        public async Task Insert(List<TModel> entityList)
        {
            foreach (TModel model in entityList)
                await Insert(model);
        }

        public async Task Save()
        {
            // throw new NotImplementedException();
        }

        public async Task Update(TModel entity)
        {
            await mongoCollection.ReplaceOneAsync(x => x.Id.Equals(entity.Id), entity);
        }

        public async Task Update(List<TModel> entityList)
        {
            foreach (TModel model in entityList)
                await Update(model);
        }
    }
}
