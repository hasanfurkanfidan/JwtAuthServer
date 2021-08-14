using JwtAuthServer.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthServer.Data.EntityFrameworkCore.Repositories
{
    public class EfGenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, new()
    {
        private readonly DbContext _context;
        private readonly DbSet<TEntity> _dbSet;
        public EfGenericRepository(AppDbContext appDbContext)
        {
            _context = appDbContext;
            _dbSet = appDbContext.Set<TEntity>();

        }
        public async Task AddAsync(TEntity entity)
        {
            await _context.AddAsync(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            var entity =await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }
            return entity;
        }

        public void Remove(TEntity entity)
        {
            _context.Remove(entity);
        }

        public TEntity Update(TEntity entity)
        {
            _context.Update(entity);
            return entity;
        }

        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression)
        {
            return _dbSet.Where(expression);
        }
    }
}
