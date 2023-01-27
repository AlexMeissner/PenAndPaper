using Microsoft.EntityFrameworkCore;

namespace Server.Database
{
    public class SQLDatabase : DbContext
    {
        public DbSet<DbActiveCampaignElements> ActiveCampaignElements { get; set; }
        public DbSet<DbUser> Users { get; set; }
        public DbSet<DbCampaign> Campaigns { get; set; }
        public DbSet<DbDiceRoll> DiceRolls { get; set; }
        public DbSet<DbCampaignUpdates> CampaignUpdates { get; set; }
        public DbSet<DbUserInCampaign> UsersInCampaign { get; set; }
        public DbSet<DbMap> Maps { get; set; }

        public SQLDatabase(DbContextOptions<SQLDatabase> options) : base(options)
        {
        }
    }
}