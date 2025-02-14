using Backend.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Database;

public class PenAndPaperDatabase(DbContextOptions<PenAndPaperDatabase> options) : DbContext(options)
{
    public DbSet<Campaign> Campaigns { get; set; }
    public DbSet<Character> Characters { get; set; }
    public DbSet<Map> Maps { get; set; }
    public DbSet<Monster> Monsters { get; set; }
    public DbSet<Token> Tokens { get; set; }
    public DbSet<CharacterToken> CharacterTokens { get; set; }
    public DbSet<MonsterToken> MonsterTokens { get; set; }
    public DbSet<User> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Campaign>()
            .HasOne(c => c.ActiveMap)
            .WithOne()
            .HasForeignKey<Campaign>(c => c.ActiveMapId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Campaign>()
            .HasMany(c => c.Maps)
            .WithOne()
            .HasForeignKey(m => m.CampaignId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}