using Microsoft.EntityFrameworkCore;
using Store.Core.Entities;
using Store.Core.Repositories;
using Store.Data.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.Data.Repostiories
{
    public class EntityRepository<TEntity>:IEntityRepository<TEntity> where TEntity : class
    {
        private readonly StoreDbContext _context;

        public EntityRepository(StoreDbContext context)
        {
            this._context = context;
        }
        public async Task AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
        }

        public bool Any(Expression<Func<TEntity, bool>> exp, params string[] Includings)
        {
            var query = _context.Set<TEntity>().AsQueryable();

            if (Includings != null)
            {
                foreach (var prop in Includings)
                {
                    query = query.Include(prop);
                }
            }


            return query.Any(exp);
        }

        public int Commit()
        {
            return _context.SaveChanges();
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Delete(TEntity category)
        {
            _context.Set<TEntity>().Remove(category);
        }

        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> exp, params string[] Includings)
        {
            var query = _context.Set<TEntity>().AsQueryable();

            if (Includings != null)
            {
                foreach (var prop in Includings)
                {
                    query = query.Include(prop);
                }
            }


            return query.Where(exp);
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> exp, params string[] Includings)
        {
            var query = _context.Set<TEntity>().AsQueryable();


            if (Includings != null)
            {
                foreach (var prop in Includings)
                {
                    query = query.Include(prop);
                }
            }

            return await query.FirstOrDefaultAsync(exp);
        }
    }
}
