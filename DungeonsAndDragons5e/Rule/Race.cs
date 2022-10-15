namespace DungeonsAndDragons5e.Rule
{
    public sealed class Race
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public AbilityScore AbilityScoreIncrease { get; set; }
        public int AdditionalAbilityScorePoints { get; set; }
        public int MaxAge { get; set; }
        public string AlignmentAdvice { get; set; }
        public int MinSize { get; set; }
        public int MaxSize { get; set; }
        public SizeCategory SizeCategory { get; set; }
        public float Speed { get; set; }
        public List<Trait> Traits { get; set; }
        public List<string> Languages { get; set; }
        public int AdditionalLanguages { get; set; }
        public List<SubRace> SubRaces { get; set; }

        public Race(string name, string description, AbilityScore abilityScoreIncrease, int additionalAbilityScorePoints, int maxAge, string alignmentAdvice, int minSize, int maxSize, SizeCategory sizeCategory, float speed, List<Trait> traits, List<string> languages, int additionalLanguages, List<SubRace> subRaces)
        {
            Name = name;
            Description = description;
            AbilityScoreIncrease = abilityScoreIncrease;
            AdditionalAbilityScorePoints = additionalAbilityScorePoints;
            MaxAge = maxAge;
            AlignmentAdvice = alignmentAdvice;
            MinSize = minSize;
            MaxSize = maxSize;
            SizeCategory = sizeCategory;
            Speed = speed;
            Traits = traits;
            Languages = languages;
            AdditionalLanguages = additionalLanguages;
            SubRaces = subRaces;
        }
    }

    public sealed class SubRace
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public AbilityScore AbilityScoreIncrease { get; set; }
        public List<Trait> Traits { get; set; }

        public SubRace(string name, string description, AbilityScore abilityScoreIncrease, List<Trait> traits)
        {
            Name = name;
            Description = description;
            AbilityScoreIncrease = abilityScoreIncrease;
            Traits = traits;
        }
    }
}

// TODO: Halbelfen bekommen noch 2x 1 beliebigen Attributpunkt dazu
// TODO: Menschen und Halbelfen können eine zusätzliche beliebige Sprache erlernen