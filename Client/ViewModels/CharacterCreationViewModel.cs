using System;
using System.Collections.ObjectModel;
using System.Reflection;
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

    public class CharacterCreationViewModel : BaseViewModel
    {
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
    }
}
