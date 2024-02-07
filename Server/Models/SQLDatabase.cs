using Microsoft.EntityFrameworkCore;

namespace Server.Models
{
    public class SQLDatabase(DbContextOptions<SQLDatabase> options) : DbContext(options)
    {
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<Map> Maps { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Sound> Sounds { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<Monster> Monsters { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<CharacterToken> CharacterTokens { get; set; }
        public DbSet<MonsterToken> MonsterTokens { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<CharacterNote> CharacterNotes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Token>().UseTptMappingStrategy();
            modelBuilder.Entity<Note>().UseTptMappingStrategy();
        }
    }
}
