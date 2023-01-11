using Store.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Repositories
{
    public interface IEntityRepository<TEntity>
    {
        Task AddAsync(TEntity entity);
        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> exp, params string[] Includings);
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> exp, params string[] Includings);
        bool Any(Expression<Func<TEntity, bool>> exp, params string[] Includings);
        void Delete(TEntity category);
        int Commit();
        Task<int> CommitAsync();
    }
}
