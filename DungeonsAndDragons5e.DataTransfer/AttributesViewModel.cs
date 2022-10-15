namespace DungeonsAndDragons5e.DataTransfer
{
    public class AttributesViewModel
    {
        // Attribute Modificators
        public int StrengthModificator { get; set; }
        public int DexterityModificator { get; set; }
        public int ConstitutionModificator { get; set; }
        public int IntelligenceModificator { get; set; }
        public int WisdomModificator { get; set; }
        public int CharismaModificator { get; set; }

        // Passive Perception
        public int PassivePerception { get; set; }

        // Saving Throws
        public int StrengthSavingThrow { get; set; }
        public int DexteritySavingThrow { get; set; }
        public int ConstitutionSavingThrow { get; set; }
        public int IntelligenceSavingThrow { get; set; }
        public int WisdomSavingThrow { get; set; }
        public int CharismaSavingThrow { get; set; }

        // Skills
        public int Acrobatics { get; set; }
        public int AnimalHandling { get; set; }
        public int Arcana { get; set; }
        public int Athletics { get; set; }
        public int Deception { get; set; }
        public int History { get; set; }
        public int Insight { get; set; }
        public int Intimidation { get; set; }
        public int Investigation { get; set; }
        public int Medicine { get; set; }
        public int Nature { get; set; }
        public int Perception { get; set; }
        public int Performance { get; set; }
        public int Persuasion { get; set; }
        public int Religion { get; set; }
        public int SlightOfHand { get; set; }
        public int Stealth { get; set; }
        public int Survival { get; set; }
    }
}