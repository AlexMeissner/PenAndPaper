using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Server.Models;
using System.Linq.Expressions;

namespace Server.Services
{
    public interface IRepository<T> where T : class
    {
        Task<T?> FirstOrDefaultAsync();
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> ToListAsync();
        Task<T?> FindAsync(int id);
        IQueryable<T> Include(string navigationPropertyPath);
        IIncludableQueryable<T, TProperty> Include<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath);
        IQueryable<TResult> Select<TResult>(Expression<Func<T, TResult>> selector);
        IQueryable<T> Where(Expression<Func<T, bool>> predicate);
    }

    public class Repository<T>(SQLDatabase context) : IRepository<T> where T : class
    {
        private readonly DbSet<T> _set = context.Set<T>();

        public async Task<T?> FirstOrDefaultAsync()
        {
            return await _set.FirstOrDefaultAsync();
        }

        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _set.FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<T>> ToListAsync()
        {
            return await _set.ToListAsync();
        }

        public async Task<T?> FindAsync(int id)
        {
            return await _set.FindAsync(id);
        }

        public IQueryable<T> Include(string navigationPropertyPath)
        {
            return _set.Include(navigationPropertyPath);
        }

        public IIncludableQueryable<T, TProperty> Include<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath)
        {
            return _set.Include(navigationPropertyPath);
        }

        public IQueryable<TResult> Select<TResult>(Expression<Func<T, TResult>> selector)
        {
            return _set.Select(selector);
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return _set.Where(predicate);
        }
    }
}
