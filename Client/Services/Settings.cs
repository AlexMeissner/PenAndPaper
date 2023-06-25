namespace Client.Services
{
    public enum Theme
    {
        Light,
        Dark,
    }

    public class AudioSettings
    {
        public float AmbientVolume { get; set; } = 1.0f;
        public float EffectsVolume { get; set; } = 1.0f;
    }

    public interface ISettings
    {
        Theme Theme { get; set; }
        AudioSettings Audio { get; set; }
    }

    public class Settings : ISettings
    {
        public Theme Theme { get; set; }
        public AudioSettings Audio { get; set; } = new();
    }
}
