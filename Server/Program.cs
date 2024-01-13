using Microsoft.EntityFrameworkCore;
using Server.Middleware;
using Server.Models;
using Server.Services;
using Server.Services.BusinessLogic;

class ServerMain
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.CreateLogger();

        var dbPath = GetDatabasePath("Database.db");

        builder.Services.AddDbContext<SQLDatabase>(options => options.UseSqlite($"Data Source={dbPath}"));
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddSingleton<IUpdateNotifier, UpdateNotifier>();

        builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        builder.Services.AddScoped<ICampaign, Campaign>();
        builder.Services.AddScoped<IScript, Script>();
        builder.Services.AddScoped<ISound, Sound>();
        builder.Services.AddScoped<IMonster, Monster>();

        var app = builder.Build();

        // SWAGGER: Automatically open Browser -> Server > Properties > Debug > Open Debug launch profile UI > Launch browser
        // https://localhost:7099/swagger/index.html
#if DEBUG
        app.UseSwagger();
        app.UseSwaggerUI();
#endif

        app.UseMiddleware<HttpLogger>();

        app.UseWebSockets();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        MigrateDatabase(app);

        app.Run();
    }

    private static void MigrateDatabase(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;

        try
        {
            var dbContext = services.GetRequiredService<SQLDatabase>();
            dbContext.Database.Migrate();
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<ServerMain>>();
            logger.LogError(ex, "An error occurred applying migrations.");
        }
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
