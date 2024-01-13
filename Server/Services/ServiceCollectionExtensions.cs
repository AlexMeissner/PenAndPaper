using Microsoft.EntityFrameworkCore;
using Server.Models;
using Server.Services.BusinessLogic;

namespace Server.Services
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddScopedServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<ICampaign, Campaign>();
            services.AddScoped<IScript, Script>();
            services.AddScoped<ISound, Sound>();
            services.AddScoped<IMonster, Monster>();

            return services;
        }

        public static IServiceCollection AddSingletonServices(this IServiceCollection services)
        {
            services.AddSingleton<IUpdateNotifier, UpdateNotifier>();

            return services;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfigurationManager configurationManager)
        {
#if DEBUG
            var dbPath = GetDatabasePath("Database.db");
            services.AddDbContext<SQLDatabase>(options => options.UseSqlite($"Data Source={dbPath}"));
#else
            var connectionString = configurationManager.GetConnectionString("Database");
            services.AddDbContext<SQLDatabase>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
#endif
            return services;
        }

        private static string GetDatabasePath(string databaseName)
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var databasePath = Path.Combine(basePath, databaseName);

            if (File.Exists(databasePath))
            {
                return databasePath;
            }

            databasePath = Path.Combine(basePath, "..", "..", "..", "..", databaseName);

            if (File.Exists(databasePath))
            {
                return databasePath;
            }

            throw new FileNotFoundException("Database file not found.");
        }
    }
}
