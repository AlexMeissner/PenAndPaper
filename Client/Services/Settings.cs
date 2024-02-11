using Microsoft.Extensions.Configuration;
using System;
using static Client.Services.ServiceExtension;

namespace Client.Services
{
    public interface ISettings
    {
        event EventHandler<float> AmbientVolumeChanged;
        event EventHandler<float> EffectVolumeChanged;

        string APIHost { get; set; }
        string WebsocketHost { get; set; }

        float AmbientVolume { get; set; }
        float EffectVolume { get; set; }
    }

    [SingletonService]
    public class Settings(IConfiguration configuration) : ISettings
    {
        public event EventHandler<float>? AmbientVolumeChanged;
        public event EventHandler<float>? EffectVolumeChanged;

        public string APIHost
        {
            get
            {
                var host = configuration["Host"];

                return host is null
                    ? throw new ArgumentNullException("Host property not found in appsettings.json.")
                    : string.Format("https://{0}/", host);
            }
            set
            {
                configuration["Host"] = value;
            }
        }

        public string WebsocketHost
        {
            get
            {
                var host = configuration["Host"];

                return host is null
                    ? throw new ArgumentNullException("Host property not found in appsettings.json.")
                    : string.Format("wss://{0}/", host);
            }
            set
            {
                configuration["Host"] = value;
            }
        }

        public float AmbientVolume
        {
            get
            {
                var property = configuration["Sound:AmbientVolume"]
                    ?? throw new ArgumentNullException("'Sound:AmbientVolume' property not found in appsettings.json.");

                if (float.TryParse(property, out float volume))
                {
                    return volume;
                }

                throw new FormatException("'Sound:AmbientVolume' property in appsettings.json is not of type float.");
            }
            set
            {
                configuration["Sound:AmbientVolume"] = value.ToString();
                AmbientVolumeChanged?.Invoke(this, value);
            }
        }

        public float EffectVolume
        {
            get
            {
                var property = configuration["Sound:EffectVolume"]
                    ?? throw new ArgumentNullException("'Sound:EffectVolume' property not found in appsettings.json.");

                if (float.TryParse(property, out float volume))
                {
                    return volume;
                }

                throw new FormatException("'Sound:EffectVolume' property in appsettings.json is not of type float.");
            }
            set
            {
                configuration["Sound:EffectVolume"] = value.ToString();
                EffectVolumeChanged?.Invoke(this, value);
            }
        }
    }
}
