using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Services
{
    public static class WebApplicationExtensions
    {
        public static void MigrateDatabase(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var dbContext = services.GetRequiredService<SQLDatabase>();
            dbContext.Database.Migrate();
        }

        public static void UpdateRulesInDatabase(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var dbContext = services.GetRequiredService<SQLDatabase>();
            var rulesParser = services.GetRequiredService<IDungeonsAndDragonsParser>();
            rulesParser.UpdateFromResources();
        }
    }
}
