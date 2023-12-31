namespace DataTransfer.Monster
{
    public enum SizeCategory
    {
        Fine, // Fein
        Diminutive, // Zierlich
        Tiny, // Winzig
        Small, // Klein,
        Medium, // Mittelgroß
        Large, // Groß
        Huge, // Riesig
        Gargantuan, // Gigantisch
        Colossal // Kolossal
    }

    public static class SizeCategoryExtension
    {
        public static string Translate(this SizeCategory category)
        {
            return category switch
            {
                SizeCategory.Fine => "Fein",
                SizeCategory.Diminutive => "Zierlich",
                SizeCategory.Tiny => "Winzig",
                SizeCategory.Small => "Klein",
                SizeCategory.Medium => "Mittelgroß",
                SizeCategory.Large => "Groß",
                SizeCategory.Huge => "Riesig",
                SizeCategory.Gargantuan => "Gigantisch",
                SizeCategory.Colossal => "Klossal",
                _ => throw new ArgumentException($"The entry {category} has no translation."),
            };
        }
    }
}
