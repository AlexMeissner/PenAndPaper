using Microsoft.EntityFrameworkCore;
using Website.Database.Rules;

namespace Website.Database;

public static class DatabaseExtensions
{
    public static void AddDatabase(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("PostgresDb");
        builder.Services.AddDbContextFactory<PenAndPaperDatabase>(options => options.UseNpgsql(connectionString));
    }

    public static void MigrateDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var factory = services.GetRequiredService<IDbContextFactory<PenAndPaperDatabase>>();
        using var dbContext = factory.CreateDbContext();
        dbContext.Database.Migrate();
    }

    public static void LoadDungeonsAndDragonsRules(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var monsterParser = services.GetRequiredService<IMonsterParser>();
        monsterParser.UpdateFromResources();
    }
}
