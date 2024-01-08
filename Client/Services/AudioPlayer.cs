using Client.Services.API;
using DataTransfer;
using NAudio.Wave;
using System;
using static Client.Services.ServiceExtension;

namespace Client.Services
{
    public interface IAudioPlayer
    {
        void Play(int id);

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
        private readonly ISessionData _sessionData;
        private readonly ISoundApi _soundApi;
        private readonly IActiveSoundApi _activeSoundApi;

        public AudioPlayer(IUpdateNotifier campaignUpdates, ICache cache, ISessionData sessionData, ISoundApi soundApi, IActiveSoundApi activeSoundApi)
        {
            _cache = cache;
            _sessionData = sessionData;
            _soundApi = soundApi;
            _activeSoundApi = activeSoundApi;

            campaignUpdates.AmbientSoundChanged += OnAmbientSoundChanged;
            campaignUpdates.SoundEffectChanged += OnSoundEffectChanged;
        }

        public async void Play(int id)
        {
            var soundResponse = await _soundApi.GetAsync(id);

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
                            });
                    }

                    var waveProvider = new Mp3FileReader(_cache.GetPath(CacheType.SoundEffect, filename));
                    var waveOut = new WaveOut();
                    waveOut.Init(waveProvider);
                    waveOut.Play();
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
            WaveOut.Play();
        }

        private void Stop()
        {
            WaveOut?.Stop();
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

                    if (success.AmbientId == -1)
                    {
                        return;
                    }

                    var soundResponse = await _soundApi.GetAsync(success.AmbientId);

                    soundResponse.Match(
                        async s =>
                        {
                            var filename = string.Format("{0}.{1}", s.Id, "mp3");

                            if (!_cache.Contains(CacheType.AmbientSound, filename) ||
                                Checksum.CreateHash(await _cache.GetData(CacheType.AmbientSound, filename)) != s.Checksum)
                            {
                                var soundDataResponse = await _soundApi.GetDataAsync(success.AmbientId);
                                soundDataResponse.Match(async x => await _cache.Add(CacheType.AmbientSound, filename, x.Data));
                            }

                            Play(_cache.GetPath(CacheType.AmbientSound, filename));
                        },
                        f => { });
                },
                failure => { });
        }

        private async void OnSoundEffectChanged(object? sender, EventArgs e)
        {
            var response = await _activeSoundApi.GetSoundEffectAsync(_sessionData.CampaignId);

            response.Match(
                success =>
                {
                    if (success.EffectId != -1)
                    {
                        Play(success.EffectId);
                    }
                },
                failure => { });
        }
    }
}