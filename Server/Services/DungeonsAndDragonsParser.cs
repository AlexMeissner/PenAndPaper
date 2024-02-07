using Server.Models;
using System.Reflection;
using System.Text.Json;

namespace Server.Services
{
    public interface IDungeonsAndDragonsParser
    {
        string LoadFromDatabase();
        void UpdateFromResources();
    }

    public class DungeonsAndDragonsParser(SQLDatabase dbContext) : IDungeonsAndDragonsParser
    {
        public string LoadFromDatabase()
        {
            var monsters = dbContext.Monsters.ToList();
            return JsonSerializer.Serialize(monsters);
        }

        public void UpdateFromResources()
        {
            var monsterSerialized = LoadResource("Server.Resources.Monsters.json");

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
            Console.WriteLine("Updating monster database...");

            var properties = typeof(Monster).GetProperties();
            var propertiesToUpdate = properties.Where(p => p.Name != "Id" && p.Name != "Tokens");

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
                        Console.WriteLine("Updating monster ({0})...", monster.Name);
                    }
                }
                else
                {
                    dbContext.Add(monster);
                    Console.WriteLine("Adding new monster ({0})...", monster.Name);
                }
            }

            dbContext.SaveChanges();

            Console.WriteLine("Monster database update finished");
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
}
