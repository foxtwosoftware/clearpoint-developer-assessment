using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TodoList.Api.DataAccess
{
    public abstract class GenericRepository<T> : IRepository<T> where T : class
    {
        protected TodoContext _context;

        public GenericRepository(TodoContext context)
        {
            _context = context;
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context
                .Set<T>()
                .AsQueryable()
                .Where(predicate)
                .ToListAsync();
        }

        public virtual async Task<T> GetAsync(Guid id)
        {
            return await _context.FindAsync<T>(id);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            EntityEntry entityEntry = _context.Entry(entity);
            entityEntry.State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return entity;
        }
    }
}
