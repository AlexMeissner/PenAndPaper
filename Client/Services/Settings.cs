using System;
using System.ComponentModel;
using static Client.Services.ServiceExtension;

namespace Client.Services
{
    public interface ISettings : INotifyPropertyChanged
    {
        event EventHandler<float> AmbientVolumeChanged;
        event EventHandler<float> EffectVolumeChanged;

        string APIHost { get; }
        string WebsocketHost { get; }

        float AmbientVolume { get; set; }
        float EffectVolume { get; set; }
    }

    [SingletonService]
    public class Settings : ISettings
    {
        public event EventHandler<float>? AmbientVolumeChanged;
        public event EventHandler<float>? EffectVolumeChanged;
        public event PropertyChangedEventHandler? PropertyChanged;

        public string APIHost => string.Format("https://{0}/", Host);

        public string WebsocketHost => string.Format("wss://{0}/", Host);

        public float AmbientVolume
        {
            get
            {
                return Properties.Settings.Default.AmbientSoundVolume;
            }
            set
            {
                Properties.Settings.Default.AmbientSoundVolume = value;
                Properties.Settings.Default.Save();
                OnPropertyChanged(nameof(AmbientVolume));
                AmbientVolumeChanged?.Invoke(this, value);
            }
        }

        public float EffectVolume
        {
            get
            {
                return Properties.Settings.Default.SoundEffectVolume;
            }
            set
            {
                Properties.Settings.Default.SoundEffectVolume = value;
                Properties.Settings.Default.Save();
                OnPropertyChanged(nameof(EffectVolume));
                EffectVolumeChanged?.Invoke(this, value);
            }
        }

        private static string Host
        {
            get
            {
#if DEBUG
                return "localhost:7099";
#else
                return Properties.Settings.Default.Host;
#endif
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
