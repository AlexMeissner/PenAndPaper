using Backend.Database;
using Backend.Database.Models;
using System.Reflection;
using System.Text.Json;

namespace Backend.DungeonsAndDragons;

public interface IMonsterParser
{
    void UpdateFromResources();
}

public class MonsterParser(ILogger<MonsterParser> logger, PenAndPaperDatabase dbContext) : IMonsterParser
{
    public void UpdateFromResources()
    {
        var monsterSerialized = LoadResource("Backend.DungeonsAndDragons.Monsters.json");

        var monsters = JsonSerializer.Deserialize<IEnumerable<Monster>>(monsterSerialized)
                ?? throw new NullReferenceException("Could not deserialize monsters");

        UpdateDatabase(monsters);
    }

    private static string LoadResource(string path)
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream(path) ?? throw new NullReferenceException("Resource not found: " + path);
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    private void UpdateDatabase(IEnumerable<Monster> monsters)
    {
        logger.LogInformation("Updating monster database...");

        var properties = typeof(Monster).GetProperties();
        var propertiesToUpdate = properties.Where(p => p.Name != "Id" && p.Name != "Tokens");

        var c = dbContext.Monsters.Count();
        logger.LogInformation("Monster count: {monsterCount}", c);

        foreach (var monster in monsters)
        {
            var monsterInDatabase = dbContext.Monsters.FirstOrDefault(m => m.Name == monster.Name);

            if (monsterInDatabase is not null)
            {
                bool updated = false;

                foreach (var property in propertiesToUpdate)
                {
                    var newValue = property.GetValue(monster);
                    var currentValue = property.GetValue(monsterInDatabase);

                    if (newValue is not null && !AreEqual(newValue, currentValue))
                    {
                        property.SetValue(monsterInDatabase, newValue);
                        updated = true;
                    }
                }

                if (updated)
                {
                    logger.LogInformation("Updating monster ({monsterName})...", monster.Name);
                }
            }
            else
            {
                dbContext.Add(monster);
                logger.LogInformation("Adding new monster ({monsterName})...", monster.Name);
            }
        }

        dbContext.SaveChanges();

        logger.LogInformation("Monster database update finished");
    }

    private static bool AreEqual(object? lhs, object? rhs)
    {
        if (lhs == null || rhs == null)
        {
            return lhs == rhs;
        }

        if (lhs is byte[] byteArray1 && rhs is byte[] byteArray2)
        {
            return byteArray1.SequenceEqual(byteArray2);
        }

        return lhs.Equals(rhs);
    }
}
