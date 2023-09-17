using Client.Commands;
using Client.Converter;
using Client.Services;
using Client.Services.API;
using DataTransfer.Character;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using static Client.Services.ServiceExtension;

namespace Client.ViewModels
{
    public class ClassPreview
    {
        public Class Class { get; }
        public BitmapImage Icon { get; set; }
        public BitmapImage Artwork { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PrimaryAbility { get; set; }
        public string HitDice { get; set; }
        public string ArmorProficiencies { get; set; }
        public string WeaponProficiencies { get; set; }

        public ClassPreview(
            Class dndClass,
            string name,
            string resourceName,
            string description,
            string primaryAbility,
            string hitDice,
            string armorProficiencies,
            string weaponProficiencies)
        {
            Class = dndClass;
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
        public Race Race { get; }
        public BitmapImage Artwork { get; set; }
        public string Name { get; set; }
        public ObservableCollection<string> Traits { get; set; }

        public RacePreview(Race race, string name, string resourceName, IList<string> traits)
        {
            Race = race;
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

    public class CharacterDetails : BaseViewModel
    {
        public string Name { get; set; } = string.Empty;
        public BitmapImage? Image { get; set; }
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Constitution { get; set; }
        public int Intelligence { get; set; }
        public int Wisdom { get; set; }
        public int Charisma { get; set; }

        public ICommand ImageCommand { get; }

        public CharacterDetails()
        {
            ImageCommand = new RelayCommand(LoadImage);
        }

        private void LoadImage()
        {
            var fileDialog = new OpenFileDialog()
            {
                Filter = "Bilder (*.png;*.jpeg)|*.png;*.jpeg"
            };

            if (fileDialog.ShowDialog() == true)
            {
                Image = new BitmapImage(new Uri(fileDialog.FileName));
            }
        }
    }

    [TransistentService]
    public class CharacterCreationViewModel : BaseViewModel
    {
        private readonly ICharacterApi _characterApi;
        private readonly ISessionData _sessionData;
        private readonly IPopupPage _popupPage;

        public ClassPreview? SelectedClass { get; set; }
        public ObservableCollection<ClassPreview> Classes { get; set; } = new()
        {
            new(Class.Barbarian,
                "Barbar",
                "barbarian",
                "Ein wilder Krieger, der in einen Kampfrausch verfallen kann",
                "Stärke",
                "W12",
                "leichte Rüstungen, mittelschwere Rüstungen, Schilde",
                "einfache Waffen, Kriegswaffen"),

            new(Class.Bard,
                "Barde",
                "bard",
                "Ein inspirierender Zauberer, dessen Kraft in seiner Musik wiederhallt",
                "Charisma",
                "W8",
                "leichte Rüstungen",
                "einfache Waffen, Handarmbrüste, Langschwerter, Rapiere, Kurzschwerter"),

            new(Class.Cleric,
                "Kleriker",
                "cleric",
                "Ein priesterlicher Champion, der göttliche Magie im Dienste einer höheren Macht einsetzt",
                "Weisheit",
                "W8",
                "leichte Rüstungen, mittelschwere Rüstungen, Schilde",
                "einfache Waffen"),

            new(Class.Druid,
                "Druide",
                "druid",
                "Ein Priester des alten Glaubens, der die Kräfte der Natur nutzt und Tierformen annimmt",
                "Weisheit",
                "W8",
                "leichte Rüstungen, mittelschwere Rüstungen, Schilde",
                "Knüppel, Dolche, Wurfpfeile, Wurfspeere, Streitkolben, Kampfstäbe, Krummsäbel, Sichel, Schleuder, Speere"),

            new(Class.Fighter,
                "Kämpfer",
                "fighter",
                "Ein Meister des Kampfes, geübt im Umgang mit einer Vielzahl von Waffen und Rüstungen",
                "Stärke oder Geschicklichkeit",
                "W10",
                "alle Rüstungen, Schilde",
                "einfache Waffen, Kriegswaffen"),

            new(Class.Monk,
                "Mönch",
                "monk",
                "Ein Meister der Kampfkünste, der die Kraft des Körpers nutzt, um körperliche und spirituelle Perfektion zu erreichen",
                "Geschicklichkeit & Weisheit",
                "W8",
                "keine",
                "einfache Waffen, Kurzschwerter"),

            new(Class.Paladin,
                "Paladin",
                "paladin",
                "Ein heiliger Krieger, der an einen heiligen Eid gebunden ist",
                "Stärke & Charisma",
                "W10",
                "alle Rüstungen, Schilde",
                "einfache Waffen, Kriegswaffen"),

            new(Class.Ranger,
                "Waldläufer",
                "ranger",
                "Ein Krieger, der Bedrohungen am Rande der Zivilisation bekämpft",
                "Geschicklichkeit & Weisheit",
                "W10",
                "leichte Rüstungen, mittelschwere Rüstungen, Schilde",
                "einfache Waffen, Kriegswaffen"),

            new(Class.Rogue,
                "Schurke",
                "rogue",
                "Ein Schurke, der mithilfe von Heimlichkeit und List Hindernisse und Feinde überwindet",
                "Geschicklichkeit",
                "W8",
                "leichte Rüstungen",
                "einfache Waffen, Handarmbrüste, Langschwerter, Rapiere, Kurzschwerter"),

            new(Class.Sorcerer,
                "Zauberer",
                "sorcerer",
                "Ein Zauberer, der die innewohnende Magie einer Gabe oder Blutlinie nutzt",
                "Charisma",
                "W6",
                "keine",
                "Dolche, Wurfpfeile, Schleudern, Kampfstäbe, leichte Armbrüste"),

            new(Class.Warlock,
                "Hexenmeister",
                "warlock",
                "Ein Träger von Magie, der aus einem Geschäft mit einem außerplanaren Wesen resultiert",
                "Charisma",
                "W8",
                "leichte Rüstungen",
                "einfache Waffen"),

            new(Class.Wizard,
                "Magier",
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
            new(Race.Elf, "Elf (Waldelfen)", "elf", new[]{ "+2 Geschicklichkeit", "+1 Weisheit", "Dunkelsicht", "Geschärfte Sinne", "Feenblut", "Trance", "Elfische Waffenvertrautheit", "Flinkheit", "Deckmantel der Wildnis" }),
            new(Race.Elf, "Elf (Drow)", "elf", new[]{ "+2 Geschicklichkeit", "+1 Charisma", "Überlegene Dunkelsicht", "Geschärfte Sinne", "Feenblut", "Trance", "Empfindlichkeit gegenüber Sonnenlicht", "Drow Magie", "Drow Waffenvertrautheit" }),
            new(Race.Halfling, "Halblinge", "halfling", new[]{ "+2 Geschicklichkeit", "Halblingsglück", "Tapferkeit", "Halblingsgewandheit", "+1 Charisma & Angeborene Verstohlenheit oder +1 Konstitution & Unempfindlichkeit" }),
            new(Race.Human, "Menschen", "human", new[]{ "+1 alle Attribute", "Zusätzliche Sprache" }),
            new(Race.Dwarf, "Zwerge (Gebirgszwerg)", "dwarf", new[]{ "+2 Konstitution", "+2 Stärke", "Dunkelsicht", "Zwergische Unverwüstlichkeit", "Zwergisches Kampftraining", "Zwergische Rüstungsvertrautheit", "Handwerkliches Geschick", "Steingespür" }),
            new(Race.Dwarf, "Zwerge (Hügelzwerge)", "dwarf", new[]{ "+2 Konstitution", "+1 Weisheit", "Dunkelsicht", "Zwergische Unverwüstlichkeit", "Zwergisches Kampftraining", "Zwergische Zähigkeit", "Handwerkliches Geschick", "Steingespür" }),
            new(Race.Dragonborn, "Drachenblütige", "dragonborn", new[]{ "+2 Stärke", "+1 Charisma", "Drakonische Abstammung", "Odemwaffe", "Schadensresistenz" }),
            new(Race.Gnome, "Gnome (Felsgnome)", "gnome", new[]{ "+2 Intelligenz", "+1 Konstitution", "Dunkelsicht", "Gnomische Gerissenheit", "Artefaktkunde", "Tüftler" }),
            new(Race.Gnome, "Gnome (Waldgnome)", "gnome", new[]{ "+2 Intelligenz", "+1 Geschicklichkeit", "Dunkelsicht", "Gnomische Gerissenheit", "Geborene Illusionisten", "Tierflüsterer" }),
            new(Race.HalfElf, "Halbelfen", "halfElf", new[]{ "+2 Charisma", "+1 beliebiges Attribut (x2)", "Dunkelsicht", "Feenblut", "Vielseitigkeit" }),
            new(Race.HalfOrc, "Halborks", "halfOrc", new[]{ "+2 Stärke", "+1 Konstitution", "Dunkelsicht", "Bedrohlich", "Durchhaltevermögen", "Wilde Angriffe" }),
            new(Race.Tiefling, "Tieflinge", "tiefling", new[]{ "+2 Charisma", "+1 Intelligenz", "Dunkelsicht", "Höllische Resistenz", "Infernalisches Erbe" })
        };

        public CharacterDetails Details { get; set; } = new();

        public ICommand CreateCommand { get; set; }

        public CharacterCreationViewModel(ICharacterApi characterApi, ISessionData sessionData, IPopupPage popupPage)
        {
            _characterApi = characterApi;
            _sessionData = sessionData;
            _popupPage = popupPage;
            CreateCommand = new AsyncCommand(CreateCharacter);
        }

        private async Task CreateCharacter()
        {
            if (SelectedClass is null)
            {
                System.Windows.MessageBox.Show("Es wurde keine Klasse ausgewählt", "Eingabe unvollständig", MessageBoxButton.OK);
                return;
            }
            if (SelectedRace is null)
            {
                MessageBox.Show("Es wurde keine Rasse ausgewählt", "Eingabe unvollständig", MessageBoxButton.OK);
                return;
            }
            if (Details.Name.Length == 0 || Details.Image is null)
            {
                MessageBox.Show("Die Charactereigenschaften sind unvollständig", "Eingabe unvollständig", MessageBoxButton.OK);
                return;
            }

            var imageData = new ByteArrayToBitmapImageConverter().ConvertBack(Details.Image);

            var payload = new CharacterCreationDto(
                CampaignId: _sessionData.CampaignId,
                UserId: _sessionData.UserId,
                Name: Details.Name,
                Class: SelectedClass.Class,
                Race: SelectedRace.Race,
                Image: imageData,
                Strength: Details.Strength,
                Dexterity: Details.Dexterity,
                Constitution: Details.Constitution,
                Intelligence: Details.Intelligence,
                Wisdom: Details.Wisdom,
                Charisma: Details.Charisma);

            await _characterApi.PostAsync(payload);

            _popupPage.Close();
        }
    }
}
