using Client.Services.API;
using DataTransfer;
using NAudio.Wave;
using System;

namespace Client.Services
{
    public interface IAudioPlayer
    {
        void Play(int id);

        event EventHandler Finished;
        event EventHandler Stopped;
    }

    public class AudioPlayer : IAudioPlayer
    {
        public event EventHandler? Finished;
        public event EventHandler? Stopped;

        private WaveOut? WaveOut;
        private Mp3FileReader? WaveProvider;

        private readonly ICache Cache;
        private readonly ISessionData SessionData;
        private readonly ISoundApi SoundApi;
        private readonly IActiveSoundApi ActiveSoundApi;

        public AudioPlayer(ICampaignUpdates campaignUpdates, ICache cache, ISessionData sessionData, ISoundApi soundApi, IActiveSoundApi activeSoundApi)
        {
            Cache = cache;
            SessionData = sessionData;
            SoundApi = soundApi;
            ActiveSoundApi = activeSoundApi;

            campaignUpdates.AmbientSoundChanged += OnAmbientSoundChanged;
            campaignUpdates.SoundEffectChanged += OnSoundEffectChanged;
        }

        public async void Play(int id)
        {
            var sound = await SoundApi.GetAsync(id);

            if (sound.Error is not null)
            {
                return;
            }

            var filename = string.Format("{0}.{1}", sound.Data.Id, "mp3");

            if (!Cache.Contains(CacheType.SoundEffect, filename) ||
                Checksum.CreateHash(await Cache.GetData(CacheType.SoundEffect, filename)) != sound.Data.Checksum)
            {
                var soundData = await SoundApi.GetDataAsync(id);

                if (soundData.Error is not null)
                {
                    return;
                }

                await Cache.Add(CacheType.SoundEffect, filename, soundData.Data.Data);
            }

            var waveProvider = new Mp3FileReader(Cache.GetPath(CacheType.SoundEffect, filename));
            var waveOut = new WaveOut();
            waveOut.Init(waveProvider);
            waveOut.Play();
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
            if (SessionData.CampaignId is int campaignId)
            {
                var activeSound = await ActiveSoundApi.GetAsync(campaignId);

                if (activeSound.Error is not null)
                {
                    return;
                }

                var id = activeSound.Data.AmbientId;

                if (id is null || id == -1)
                {
                    Stop();
                    return;
                }

                var sound = await SoundApi.GetAsync((int)id);

                if (sound.Error is not null)
                {
                    return;
                }

                var filename = string.Format("{0}.{1}", sound.Data.Id, "mp3");

                if (!Cache.Contains(CacheType.AmbientSound, filename) ||
                    Checksum.CreateHash(await Cache.GetData(CacheType.AmbientSound, filename)) != sound.Data.Checksum)
                {
                    var soundData = await SoundApi.GetDataAsync((int)id);

                    if (soundData.Error is not null)
                    {
                        return;
                    }

                    await Cache.Add(CacheType.AmbientSound, filename, soundData.Data.Data);
                }

                Play(Cache.GetPath(CacheType.AmbientSound, filename));
            }
        }

        private async void OnSoundEffectChanged(object? sender, EventArgs e)
        {
            if (SessionData.CampaignId is int campaignId)
            {
                var activeSound = await ActiveSoundApi.GetAsync(campaignId);

                if (activeSound.Error is not null)
                {
                    return;
                }

                var id = activeSound.Data.EffectId;

                if (id is int effectId && effectId != -1)
                {
                    Play(effectId);
                }
            }
        }
    }
}