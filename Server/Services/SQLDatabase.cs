using Microsoft.EntityFrameworkCore;
using Server.Database;

namespace Server.Services
{
    public class SQLDatabase : DbContext
    {
        public SQLDatabase(DbContextOptions<SQLDatabase> options) : base(options)
        {
        }

        public DbSet<DbUser> Users { get; set; }
        public DbSet<DbCampaign> Campaigns { get; set; }
        public DbSet<DbUserInCampaign> UsersInCampaign { get; set; }
    }
}