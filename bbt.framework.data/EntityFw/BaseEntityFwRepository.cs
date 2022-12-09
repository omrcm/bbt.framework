using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace bbt.framework.data
{
    public class BaseEntityFwRepository<TModel, TDbContext> : IBaseRepository<TModel>

      where TModel : BaseDBModel, new()
        where TDbContext : DbContext, IDbContext
    {
        protected TDbContext context;
        private DbSet<TModel> entities;

        public BaseEntityFwRepository(TDbContext _context)
        {
            context = _context;
            entities = context.Set<TModel>();
        }

        public TDbContext Context
        {
            get
            {
                return context;
            }
        }

        public async Task<TModel> GetById(int id)
        {
            return await entities.FindAsync(id);
        }

        public IQueryable<TModel> GetAll()
        {
            return entities.AsNoTracking();
        }
        public IQueryable<TModel> FindBy(Expression<Func<TModel, bool>> queryExpression)
        {
            return entities.AsNoTracking().Where(queryExpression);
        }

        public Task<TModel> FirstOrDefault(Expression<Func<TModel, bool>> queryExpression)
        {

            return entities.AsNoTracking().FirstOrDefaultAsync(queryExpression);
        }

        public async Task Insert(TModel entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            await entities.AddAsync(entity);
        }
        public async Task Insert(List<TModel> entityList)
        {
            foreach (TModel entity in entityList)
            {
                await entities.AddAsync(entity);
            }
        }


        public async Task Update(TModel entity)

        {
            context.Entry(entity).State = EntityState.Modified;
        }
        public async Task Update(List<TModel> entityList)
        {
            foreach (TModel entity in entityList)
            {
                context.Entry(entity).State = EntityState.Modified;
            }
        }

        public async Task Delete(TModel entity)
        {
            entities.Remove(entity);
        }
        public async Task Delete(List<TModel> entityList)
        {
            foreach (TModel entity in entityList)
            {
                entities.Remove(entity);
            }
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task<IDbContextTransaction> BeginTransaction()
        {
            return await context.Database.BeginTransactionAsync();
        }

        public async Task EnsureCreated()
        {
            await context.Database.EnsureCreatedAsync();
        }

        public TDbContext GetDbContext()
        {
            return context;
        }
    }
}
