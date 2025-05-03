using Backend.Database;
using Backend.DungeonsAndDragons;
using Microsoft.EntityFrameworkCore;

namespace Backend.Extensions;

public static class WebApplicationExtensions
{
    public static void AddDatabase(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("PostgresDb");
        builder.Services.AddDbContext<PenAndPaperDatabase>(options => options.UseNpgsql(connectionString));
    }

    public static void MigrateDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var dbContext = services.GetRequiredService<PenAndPaperDatabase>();
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