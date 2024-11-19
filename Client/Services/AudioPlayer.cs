using Client.Services.API;
using DataTransfer;
using DataTransfer.Sound;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Client.Services.ServiceExtension;

namespace Client.Services
{
    public interface IAudioPlayer
    {
        Task Play(int id);
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

        private readonly Dictionary<WaveOut, Mp3FileReader> _fileReader = [];

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

        public async Task Play(int id)
        {
            var soundFilename = await GetSound(id, CacheType.SoundEffect);

            if (soundFilename is string filename)
            {
                var filepath = _cache.GetPath(CacheType.SoundEffect, filename);
                PlayEffect(filepath);
            }
        }

        public void Stop()
        {
            WaveOut?.Stop();
        }

        private void PlayAmbient(string filepath)
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

        private void PlayEffect(string filepath)
        {
            var waveProvider = new Mp3FileReader(filepath);
            var waveOut = new WaveOut();

            _fileReader.Add(waveOut, waveProvider);

            waveOut.Init(waveProvider);
            waveOut.Volume = _settings.EffectVolume;
            waveOut.Play();
            waveOut.PlaybackStopped += OnEffectFinished;
        }

        private void OnEffectFinished(object? sender, StoppedEventArgs e)
        {
            if (sender is WaveOut waveOut)
            {
                if (_fileReader.TryGetValue(waveOut, out var mp3FileReader))
                {
                    mp3FileReader.Dispose();
                }
                waveOut.Dispose();
            }
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
            Stop();

            var response = await _activeSoundApi.GetAmbientSoundAsync(_sessionData.CampaignId);

            var ambientId = response.Match(
                ambientSond => { return ambientSond.AmbientId; },
                errorCode => { return null; });

            if (ambientId is null)
            {
                // ToDo: Log error
                return;
            }

            var soundFilename = await GetSound((int)ambientId, CacheType.AmbientSound);

            if (soundFilename is string filename)
            {
                var filepath = _cache.GetPath(CacheType.AmbientSound, filename);
                PlayAmbient(filepath);
            }
        }

        private async void OnSoundEffectChanged(object? sender, EventArgs e)
        {
            var response = await _activeSoundApi.GetSoundEffectAsync(_sessionData.CampaignId);

            var effectId = response.Match(
                sound => { return sound.EffectId; },
                failure => { return null; });

            if (effectId is null)
            {
                // ToDo: Log error
                return;
            }

            await Play((int)effectId);
        }

        private async Task<string?> GetSound(int id, CacheType cacheType)
        {
            var soundResponse = await _soundApi.GetAsync(id);

            var sound = soundResponse.Match<SoundDto?>(
                sound => { return sound; },
                errorCode => { return null; });

            if (sound is null)
            {
                // ToDo: Log error
                return null;
            }

            var filename = string.Format("{0}.{1}", sound.Id, "mp3");
            var downloadRequired = false;

            if (_cache.Contains(cacheType, filename))
            {
                var soundData = await _cache.GetData(cacheType, filename);
                downloadRequired = Checksum.CreateHash(soundData) != sound.Checksum;
            }
            else
            {
                downloadRequired = true;
            }

            if (downloadRequired)
            {
                var soundDataResponse = await _soundApi.GetDataAsync(id);

                var soundData = soundDataResponse.Match<byte[]?>(
                    sound => { return sound.Data; },
                    errorCode => { return null; });

                if (soundData is null)
                {
                    // ToDo: Log error
                    return null;
                }

                await _cache.Add(cacheType, filename, soundData);
            }

            return filename;
        }
    }
}
