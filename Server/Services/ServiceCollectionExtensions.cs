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
            services.AddScoped<IDatabaseContext, DatabaseContext>();
            services.AddScoped<ICampaignManager, CampaignManager>();
            services.AddScoped<IScriptManager, ScriptManager>();
            services.AddScoped<ISoundManager, SoundManager>();
            services.AddScoped<IMonsterManager, MonsterManager>();
            services.AddScoped<IDungeonsAndDragonsParser, DungeonsAndDragonsParser>();

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
            services.AddDbContext<SQLDatabase>(options => options.UseSqlite($"Data Source = Database.db"));
#else
services.AddDbContext<SQLDatabase>(options => options.UseSqlite($"Data Source = Database.db"));
            //var connectionString = configurationManager.GetConnectionString("Database");
            //services.AddDbContext<SQLDatabase>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
#endif
            return services;
        }
    }
}
