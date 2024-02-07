using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Services
{
    public interface IDatabaseContext
    {
        Task AddAsync<T>(T entity) where T : class;
        Task RemoveAsync<T>(T entity) where T : class;
        Task SaveChangesAsync();
        Task UpdateAsync<T>(T entity) where T : class;
    }

    public class DatabaseContext(SQLDatabase context) : IDatabaseContext
    {
        public async Task AddAsync<T>(T entity) where T : class
        {
            await context.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task RemoveAsync<T>(T entity) where T : class
        {
            context.Remove(entity);
            await context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync<T>(T entity) where T : class
        {
            context.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
    }
}
