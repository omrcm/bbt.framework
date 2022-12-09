using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace bbt.framework.data
{
    public interface IBaseRepository<TModel>

      where TModel : BaseDBModel, new()
    {
        Task EnsureCreated();
        Task<TModel> GetById(int id);
        IQueryable<TModel> GetAll();
        IQueryable<TModel> FindBy(Expression<Func<TModel, bool>> predicate);
        //IQueryable<TModel> FromSql(string sql, params object[] parameters);
        Task<TModel> FirstOrDefault(Expression<Func<TModel, bool>> predicate);
        Task Insert(TModel entity);
        Task Insert(List<TModel> entityList);
        Task Update(TModel entity);
        Task Update(List<TModel> entityList);
        Task Delete(TModel entity);
        Task Delete(List<TModel> entityList);
        Task Save();
    }
}