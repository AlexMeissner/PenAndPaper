using Client.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Client.ViewModels
{
    public class ClassPreview
    {
        public BitmapImage Icon { get; set; }
        public BitmapImage Artwork { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PrimaryAbility { get; set; }
        public string HitDice { get; set; }
        public string ArmorProficiencies { get; set; }
        public string WeaponProficiencies { get; set; }

        public ClassPreview(
            string name,
            string resourceName,
            string description,
            string primaryAbility,
            string hitDice,
            string armorProficiencies,
            string weaponProficiencies)
        {
            Icon = LoadIcon(resourceName);
            Artwork = LoadArtwork(resourceName);
            Name = name;
            Description = description;
            PrimaryAbility = primaryAbility;
            HitDice = hitDice;
            ArmorProficiencies = armorProficiencies;
            WeaponProficiencies = weaponProficiencies;
        }

        private static BitmapImage LoadIcon(string name)
        {
            return LoadResource($"Client.Resources.Images.Icons.{name}.jpeg");
        }

        private static BitmapImage LoadArtwork(string name)
        {
            return LoadResource($"Client.Resources.Images.Artworks.{name}.png");
        }

        private static BitmapImage LoadResource(string path)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            using var resourceStream = assembly.GetManifestResourceStream(path);

            if (resourceStream != null)
            {
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = resourceStream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
                return bitmapImage;
            }
            else
            {
                throw new Exception($"Resource {path} not found.");
            }
        }
    }

    public class RacePreview
    {
        public BitmapImage Artwork { get; set; }
        public string Name { get; set; }
        public ObservableCollection<string> Traits { get; set; }

        public RacePreview(string name, string resourceName, IList<string> traits)
        {
            Artwork = LoadArtwork(resourceName);
            Name = name;
            Traits = new(traits);
        }

        // ToDo: Duplicate
        private static BitmapImage LoadArtwork(string name)
        {
            return LoadResource($"Client.Resources.Images.Artworks.{name}.png");
        }

        // ToDo: Duplicate
        private static BitmapImage LoadResource(string path)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            using var resourceStream = assembly.GetManifestResourceStream(path);

            if (resourceStream != null)
            {
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = resourceStream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
                return bitmapImage;
            }
            else
            {
                throw new Exception($"Resource {path} not found.");
            }
        }
    }

    public class CharacterDetails
    {
        public string Name { get; set; } = string.Empty;
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Constitution { get; set; }
        public int Intelligence { get; set; }
        public int Wisdom { get; set; }
        public int Charisma { get; set; }
    }

    public class CharacterCreationViewModel : BaseViewModel
    {
        public ClassPreview? SelectedClass { get; set; }
        public ObservableCollection<ClassPreview> Classes { get; set; } = new()
        {
            new("Barbar",
                "barbarian",
                "Ein wilder Krieger, der in einen Kampfrausch verfallen kann",
                "Stärke",
                "W12",
                "leichte Rüstungen, mittelschwere Rüstungen, Schilde",
                "einfache Waffen, Kriegswaffen"),

            new("Barde",
                "bard",
                "Ein inspirierender Zauberer, dessen Kraft in seiner Musik wiederhallt",
                "Charisma",
                "W8",
                "leichte Rüstungen",
                "einfache Waffen, Handarmbrüste, Langschwerter, Rapiere, Kurzschwerter"),

            new("Kleriker",
                "cleric",
                "Ein priesterlicher Champion, der göttliche Magie im Dienste einer höheren Macht einsetzt",
                "Weisheit",
                "W8",
                "leichte Rüstungen, mittelschwere Rüstungen, Schilde",
                "einfache Waffen"),

            new("Druide",
                "druid",
                "Ein Priester des alten Glaubens, der die Kräfte der Natur nutzt und Tierformen annimmt",
                "Weisheit",
                "W8",
                "leichte Rüstungen, mittelschwere Rüstungen, Schilde",
                "Knüppel, Dolche, Wurfpfeile, Wurfspeere, Streitkolben, Kampfstäbe, Krummsäbel, Sichel, Schleuder, Speere"),

            new("Kämpfer",
                "fighter",
                "Ein Meister des Kampfes, geübt im Umgang mit einer Vielzahl von Waffen und Rüstungen",
                "Stärke oder Geschicklichkeit",
                "W10",
                "alle Rüstungen, Schilde",
                "einfache Waffen, Kriegswaffen"),

            new("Mönch",
                "monk",
                "Ein Meister der Kampfkünste, der die Kraft des Körpers nutzt, um körperliche und spirituelle Perfektion zu erreichen",
                "Geschicklichkeit & Weisheit",
                "W8",
                "keine",
                "einfache Waffen, Kurzschwerter"),

            new("Paladin",
                "paladin",
                "Ein heiliger Krieger, der an einen heiligen Eid gebunden ist",
                "Stärke & Charisma",
                "W10",
                "alle Rüstungen, Schilde",
                "einfache Waffen, Kriegswaffen"),

            new("Waldläufer",
                "ranger",
                "Ein Krieger, der Bedrohungen am Rande der Zivilisation bekämpft",
                "Geschicklichkeit & Weisheit",
                "W10",
                "leichte Rüstungen, mittelschwere Rüstungen, Schilde",
                "einfache Waffen, Kriegswaffen"),

            new("Schurke",
                "rogue",
                "Ein Schurke, der mithilfe von Heimlichkeit und List Hindernisse und Feinde überwindet",
                "Geschicklichkeit",
                "W8",
                "leichte Rüstungen",
                "einfache Waffen, Handarmbrüste, Langschwerter, Rapiere, Kurzschwerter"),

            new("Zauberer",
                "sorcerer",
                "Ein Zauberer, der die innewohnende Magie einer Gabe oder Blutlinie nutzt",
                "Charisma",
                "W6",
                "keine",
                "Dolche, Wurfpfeile, Schleudern, Kampfstäbe, leichte Armbrüste"),

            new("Hexenmeister",
                "warlock",
                "Ein Träger von Magie, der aus einem Geschäft mit einem außerplanaren Wesen resultiert",
                "Charisma",
                "W8",
                "leichte Rüstungen",
                "einfache Waffen"),

            new("Magier",
                "wizard",
                "Ein gelehrter Magieanwender, der in der Lage ist, die Strukturen der Realität zu manipulieren",
                "Intelligenz",
                "W6",
                "keine",
                "Dolche, Wurfpfeile, Schleudern, Kampfstäbe, leichte Armbrüste"),
        };

        public RacePreview? SelectedRace { get; set; }
        public ObservableCollection<RacePreview> Races { get; set; } = new()
        {
            new("Elf (Waldelfen)", "elf", new[]{ "+2 Geschicklichkeit", "+1 Weisheit", "Dunkelsicht", "Geschärfte Sinne", "Feenblut", "Trance", "Elfische Waffenvertrautheit", "Flinkheit", "Deckmantel der Wildnis" }),
            new("Elf (Drow)", "elf", new[]{ "+2 Geschicklichkeit", "+1 Charisma", "Überlegene Dunkelsicht", "Geschärfte Sinne", "Feenblut", "Trance", "Empfindlichkeit gegenüber Sonnenlicht", "Drow Magie", "Drow Waffenvertrautheit" }),
            new("Halblinge", "halfling", new[]{ "+2 Geschicklichkeit", "Halblingsglück", "Tapferkeit", "Halblingsgewandheit", "+1 Charisma & Angeborene Verstohlenheit oder +1 Konstitution & Unempfindlichkeit" }),
            new("Menschen", "human", new[]{ "+1 alle Attribute", "Zusätzliche Sprache" }),
            new("Zwerge (Gebirgszwerg)", "dwarf", new[]{ "+2 Konstitution", "+2 Stärke", "Dunkelsicht", "Zwergische Unverwüstlichkeit", "Zwergisches Kampftraining", "Zwergische Rüstungsvertrautheit", "Handwerkliches Geschick", "Steingespür" }),
            new("Zwerge (Hügelzwerge)", "dwarf", new[]{ "+2 Konstitution", "+1 Weisheit", "Dunkelsicht", "Zwergische Unverwüstlichkeit", "Zwergisches Kampftraining", "Zwergische Zähigkeit", "Handwerkliches Geschick", "Steingespür" }),
            new("Drachenblütige", "dragonborn", new[]{ "+2 Stärke", "+1 Charisma", "Drakonische Abstammung", "Odemwaffe", "Schadensresistenz" }),
            new("Gnome (Felsgnome)", "gnome", new[]{ "+2 Intelligenz", "+1 Konstitution", "Dunkelsicht", "Gnomische Gerissenheit", "Artefaktkunde", "Tüftler" }),
            new("Gnome (Waldgnome)", "gnome", new[]{ "+2 Intelligenz", "+1 Geschicklichkeit", "Dunkelsicht", "Gnomische Gerissenheit", "Geborene Illusionisten", "Tierflüsterer" }),
            new("Halbelfen", "halfElf", new[]{ "+2 Charisma", "+1 beliebiges Attribut (x2)", "Dunkelsicht", "Feenblut", "Vielseitigkeit" }),
            new("Halborks", "halfOrc", new[]{ "+2 Stärke", "+1 Konstitution", "Dunkelsicht", "Bedrohlich", "Durchhaltevermögen", "Wilde Angriffe" }),
            new("Tieflinge", "tiefling", new[]{ "+2 Charisma", "+1 Intelligenz", "Dunkelsicht", "Höllische Resistenz", "Infernalisches Erbe" })
        };

        public CharacterDetails Details { get; set; } = new();

        public ICommand CreateCommand { get; set; }

        public CharacterCreationViewModel()
        {
            CreateCommand = new RelayCommand(CreateCharacter);
        }

        private void CreateCharacter()
        {

        }
    }
}
