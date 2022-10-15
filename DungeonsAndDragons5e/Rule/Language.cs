namespace DungeonsAndDragons5e.Rule
{
    public sealed class Language
    {
        public string Name { get; set; }
        public string Script { get; set; }

        public Language(string name, string script)
        {
            Name = name;
            Script = script;
        }
    }
}