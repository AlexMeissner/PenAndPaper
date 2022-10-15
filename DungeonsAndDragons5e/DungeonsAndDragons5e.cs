using DungeonsAndDragons5e.Rule;
using System.Reflection;
using System.Text.Json;

namespace DungeonsAndDragons5e
{
    public sealed class DungeonsAndDragons5e
    {
        public List<Background> Backgrounds { get; set; }
        public List<Class> Classes { get; set; }
        public List<Language> Languages { get; set; }
        public List<Race> Races { get; set; }
        public List<Spell> Spells { get; set; }

        public DungeonsAndDragons5e()
        {
            Backgrounds = LoadResource<List<Background>>("Background.json");
            Classes = LoadResource<List<Class>>("Class.json");
            Languages = LoadResource<List<Language>>("Language.json");
            Races = LoadResource<List<Race>>("Race.json");
            Spells = LoadResource<List<Spell>>("Spell.json");
        }

        private static T LoadResource<T>(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourcePath = assembly.GetManifestResourceNames().Single(x => x.EndsWith(resourceName));

            using Stream stream = assembly.GetManifestResourceStream(resourcePath)!;
            using StreamReader reader = new(stream);

            return JsonSerializer.Deserialize<T>(reader.ReadToEnd())!;
        }

        public static string Translate(SpellSchool spellSchool)
        {
            return spellSchool switch
            {
                SpellSchool.Abjuration => "Bannmagie",
                SpellSchool.Conjuration => "Beschwörung",
                SpellSchool.Divination => "Erkenntnismagie",
                SpellSchool.Enchantment => "Verzauberung",
                SpellSchool.Evocation => "Hervorrufung",
                SpellSchool.Illusion => "Illusion",
                SpellSchool.Necromancy => "Nekromantie",
                SpellSchool.Transmutation => "Verwandlung",
                _ => throw new NotImplementedException("Unknown spell school"),
            };
        }
    }
}