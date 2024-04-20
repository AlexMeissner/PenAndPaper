using Client.Services.API;
using DataTransfer;
using NAudio.Wave;
using System;
using System.Diagnostics;
using static Client.Services.ServiceExtension;

namespace Client.Services
{
    public interface IAudioPlayer
    {
        void Play(int id);
        void Stop();

        event EventHandler Finished;
        event EventHandler Stopped;
    }

    [SingletonService]
    public class AudioPlayer : IAudioPlayer
    {
        public event EventHandler? Finished;
        public event EventHandler? Stopped;

        private WaveOut? WaveOut;
        private Mp3FileReader? WaveProvider;

        private readonly ICache _cache;
        private readonly ISettings _settings;
        private readonly ISessionData _sessionData;
        private readonly ISoundApi _soundApi;
        private readonly IActiveSoundApi _activeSoundApi;

        public AudioPlayer(IUpdateNotifier campaignUpdates, ISettings settings, ICache cache, ISessionData sessionData, ISoundApi soundApi, IActiveSoundApi activeSoundApi)
        {
            _cache = cache;
            _settings = settings;
            _sessionData = sessionData;
            _soundApi = soundApi;
            _activeSoundApi = activeSoundApi;

            campaignUpdates.AmbientSoundChanged += OnAmbientSoundChanged;
            campaignUpdates.SoundEffectChanged += OnSoundEffectChanged;
            settings.AmbientVolumeChanged += OnAmbientVolumeChanged;
        }

        public async void Play(int id)
        {
            var soundResponse = await _soundApi.GetAsync(id);

            // ToDo:
            // 'waveProvider' and 'waveOut' are not disposed
            // code duplicate 'play'
            // this is a quick fix: there is an IO exception if a sound is played for the first time
            // the sound is downloaded and stored to a file while 'Mp3FileReader' already tries to access it
            soundResponse.Match(
                async success =>
                {
                    var filename = string.Format("{0}.{1}", success.Id, "mp3");

                    if (!_cache.Contains(CacheType.SoundEffect, filename) ||
                        Checksum.CreateHash(await _cache.GetData(CacheType.SoundEffect, filename)) != success.Checksum)
                    {
                        var soundDataResponse = await _soundApi.GetDataAsync(id);

                        soundDataResponse.Match(
                            async s =>
                            {
                                await _cache.Add(CacheType.SoundEffect, filename, s.Data);
                                var filepath = _cache.GetPath(CacheType.SoundEffect, filename);
                                var waveProvider = new Mp3FileReader(filepath);
                                var waveOut = new WaveOut();
                                waveOut.Init(waveProvider);
                                waveOut.Volume = _settings.EffectVolume;
                                waveOut.Play();
                            },
                            failure =>
                            {
                                Debug.WriteLine($"Failed to cache sound ({failure})");
                            });
                    }
                    else
                    {
                        var filepath = _cache.GetPath(CacheType.SoundEffect, filename);
                        var waveProvider = new Mp3FileReader(filepath);
                        var waveOut = new WaveOut();
                        waveOut.Init(waveProvider);
                        waveOut.Volume = _settings.EffectVolume;
                        waveOut.Play();
                    }
                });
        }

        private void Play(string filepath)
        {
            if (WaveOut is not null)
            {
                WaveOut.PlaybackStopped -= OnPlaybackStopped;
            }

            WaveProvider = new Mp3FileReader(filepath);
            WaveOut = new WaveOut();
            WaveOut.Init(WaveProvider);
            WaveOut.PlaybackStopped += OnPlaybackStopped;
            WaveOut.Volume = _settings.AmbientVolume;
            WaveOut.Play();
        }

        public void Stop()
        {
            WaveOut?.Stop();
        }

        private void OnAmbientVolumeChanged(object? sender, float e)
        {
            if (WaveOut is not null)
            {
                WaveOut.Volume = e;
            }
        }

        private void OnPlaybackStopped(object? sender, StoppedEventArgs e)
        {
            if (WaveOut is not null && WaveProvider is not null)
            {
                if (WaveProvider.CurrentTime == WaveProvider.TotalTime)
                {
                    Finished?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    Stopped?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private async void OnAmbientSoundChanged(object? sender, EventArgs e)
        {
            var activeSoundResponse = await _activeSoundApi.GetAmbientSoundAsync(_sessionData.CampaignId);

            activeSoundResponse.Match(
                async success =>
                {
                    Stop();

                    if (success.AmbientId is int ambientId)
                    {
                        var soundResponse = await _soundApi.GetAsync(ambientId);

                        soundResponse.Match(
                            async s =>
                            {
                                var filename = string.Format("{0}.{1}", s.Id, "mp3");

                                if (!_cache.Contains(CacheType.AmbientSound, filename) ||
                                    Checksum.CreateHash(await _cache.GetData(CacheType.AmbientSound, filename)) != s.Checksum)
                                {
                                    var soundDataResponse = await _soundApi.GetDataAsync(ambientId);
                                    soundDataResponse.Match(async x => await _cache.Add(CacheType.AmbientSound, filename, x.Data));
                                }

                                Play(_cache.GetPath(CacheType.AmbientSound, filename));
                            },
                            f => { });
                    }
                },
                failure => { });
        }

        private async void OnSoundEffectChanged(object? sender, EventArgs e)
        {
            var response = await _activeSoundApi.GetSoundEffectAsync(_sessionData.CampaignId);

            response.Match(
                success =>
                {
                    if (success.EffectId is int effectId)
                    {
                        Play(effectId);
                    }
                },
                failure => { });
        }
    }
}
