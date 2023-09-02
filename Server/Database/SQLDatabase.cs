using Microsoft.EntityFrameworkCore;

namespace Server.Database
{
    public class SQLDatabase : DbContext
    {
        public DbSet<DbActiveCampaignElements> ActiveCampaignElements { get; set; } = null!;
        public DbSet<DbUser> Users { get; set; } = null!;
        public DbSet<DbCampaign> Campaigns { get; set; } = null!;
        public DbSet<DbDiceRoll> DiceRolls { get; set; } = null!;
        public DbSet<DbUserInCampaign> UsersInCampaign { get; set; } = null!;
        public DbSet<DbMap> Maps { get; set; } = null!;
        public DbSet<DbSound> Sounds { get; set; } = null!;
        public DbSet<DbCharacter> Characters { get; set; } = null!;
        public DbSet<DbCharactersInCampaign> CharactersInCampaign { get; set; } = null!;
        public DbSet<DbToken> Tokens { get; set; } = null!;
        public DbSet<DbTokensOnMap> TokensOnMap { get; set; } = null!;

        public SQLDatabase(DbContextOptions<SQLDatabase> options) : base(options)
        {
        }
    }
}