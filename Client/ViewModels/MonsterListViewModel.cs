using Client.Converter;
using Client.Services.API;
using DataTransfer.Monster;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using static Client.Services.ServiceExtension;

namespace Client.ViewModels
{
    public class BaseMonsterData
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Size { get; init; }
        public string Type { get; init; }
        public string Alignment { get; init; }
        public int ArmorClass { get; init; }
        public int HitPoints { get; init; }
        public string HitDice { get; init; }
        public string Speed { get; init; }
        public int Strength { get; init; }
        public int Dexterity { get; init; }
        public int Constitution { get; init; }
        public int Intelligence { get; init; }
        public int Wisdom { get; init; }
        public int Charisma { get; init; }
        public int SavingThrowStrength { get; init; }
        public int SavingThrowDexterity { get; init; }
        public int SavingThrowConstitution { get; init; }
        public int SavingThrowIntelligence { get; init; }
        public int SavingThrowWisdom { get; init; }
        public int SavingThrowCharisma { get; init; }
        public int Acrobatics { get; init; }
        public int AnimalHandling { get; init; }
        public int Arcana { get; init; }
        public int Athletics { get; init; }
        public int Deception { get; init; }
        public int History { get; init; }
        public int Insight { get; init; }
        public int Intimidation { get; init; }
        public int Investigation { get; init; }
        public int Medicine { get; init; }
        public int Nature { get; init; }
        public int Perception { get; init; }
        public int Performance { get; init; }
        public int Persuasion { get; init; }
        public int Religion { get; init; }
        public int SlightOfHand { get; init; }
        public int Stealth { get; init; }
        public int Survival { get; init; }
        public string DamageResistances { get; init; }
        public string DamageImmunities { get; init; }
        public string ConditionImmunities { get; init; }
        public string Senses { get; init; }
        public string Languages { get; init; }
        public double ChallangeRating { get; init; }
        public int Experience { get; init; }
        public string Traits { get; init; }
        public string Actions { get; init; }
        public BitmapImage Image { get; init; }

        public BaseMonsterData(MonsterDto monster)
        {
            Id = monster.Id;
            Name = monster.Name;
            Size = monster.Size.Translate();
            Type = monster.Type;
            Alignment = monster.Alignment;
            ArmorClass = monster.ArmorClass;
            HitPoints = monster.HitPoints;
            HitDice = monster.HitDice;
            Speed = monster.Speed;
            Strength = monster.Strength;
            Dexterity = monster.Dexterity;
            Constitution = monster.Constitution;
            Intelligence = monster.Intelligence;
            Wisdom = monster.Wisdom;
            Charisma = monster.Charisma;
            SavingThrowStrength = monster.SavingThrowStrength;
            SavingThrowDexterity = monster.SavingThrowDexterity;
            SavingThrowConstitution = monster.SavingThrowConstitution;
            SavingThrowIntelligence = monster.SavingThrowIntelligence;
            SavingThrowWisdom = monster.SavingThrowWisdom;
            SavingThrowCharisma = monster.SavingThrowCharisma;
            Acrobatics = monster.Acrobatics;
            AnimalHandling = monster.AnimalHandling;
            Arcana = monster.Arcana;
            Athletics = monster.Athletics;
            Deception = monster.Deception;
            History = monster.History;
            Insight = monster.Insight;
            Intimidation = monster.Intimidation;
            Investigation = monster.Investigation;
            Medicine = monster.Medicine;
            Nature = monster.Nature;
            Perception = monster.Perception;
            Performance = monster.Performance;
            Persuasion = monster.Persuasion;
            Religion = monster.Religion;
            SlightOfHand = monster.SlightOfHand;
            Stealth = monster.Stealth;
            Survival = monster.Survival;
            DamageResistances = monster.DamageResistances;
            DamageImmunities = monster.DamageImmunities;
            ConditionImmunities = monster.ConditionImmunities;
            Senses = monster.Senses;
            Languages = monster.Languages;
            ChallangeRating = monster.ChallangeRating;
            Experience = monster.Experience;
            Traits = monster.Traits;
            Actions = monster.Actions;

            var converter = new ByteArrayToBitmapImageConverter();
            Image = converter.Convert(monster.Image);
        }
    }

    [TransistentService]
    public class MonsterListViewModel(IMonsterApi monsterApi) : BaseViewModel
    {
        public string Filter { get; set; } = string.Empty;

        public ObservableCollection<BaseMonsterData> Monsters { get; set; } = [];

        public bool OnFilter(object item)
        {
            if (string.IsNullOrEmpty(Filter))
            {
                return true;
            }

            if (item is BaseMonsterData monsterData)
            {
                return monsterData.Name.Contains(Filter, System.StringComparison.CurrentCultureIgnoreCase);
            }

            return false;
        }

        public async Task OnLoaded()
        {
            var monsters = await monsterApi.GetAll();
            monsters.Match(
                success =>
                {
                    var baseMonsterData = success.Items.Select(x => new BaseMonsterData(x));
                    Monsters.ReplaceWith(baseMonsterData.ToList());
                },
                failure => { });
        }
    }
}
