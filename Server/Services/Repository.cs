using Microsoft.EntityFrameworkCore;
using Server.Models;
using System.Linq.Expressions;

namespace Server.Services
{
    public interface IRepository<T> where T : class
    {
        Task AddAsync(T entity);
        Task<T?> FirstAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task RemoveAsync(T entity);
        IQueryable<TResult> Select<TResult>(Expression<Func<T, TResult>> selector);
        Task UpdateAsync(T entity);
        IQueryable<T> Where(Expression<Func<T, bool>> predicate);
    }

    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _set;

        public Repository(SQLDatabase context)
        {
            _context = context;
            _set = context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await _set.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<T?> FirstAsync(Expression<Func<T, bool>> predicate)
        {
            return await _set.FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _set.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _set.FindAsync(id);
        }

        public async Task RemoveAsync(T entity)
        {
            _set.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public IQueryable<TResult> Select<TResult>(Expression<Func<T, TResult>> selector)
        {
            return _set.Select(selector);
        }

        public async Task UpdateAsync(T entity)
        {
            _set.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return _set.Where(predicate);
        }
    }
}
