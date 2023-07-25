﻿using Client.Services.API;
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

        public AudioPlayer(ICampaignUpdates campaignUpdates, ICache cache, ISessionData sessionData, ISoundApi soundApi, IActiveSoundApi activeSoundApi)
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
            if (_sessionData.CampaignId is int campaignId)
            {
                var activeSoundResponse = await _activeSoundApi.GetAsync(campaignId);

                activeSoundResponse.Match(
                    async success =>
                    {
                        var id = success.AmbientId;

                        if (id is null || id == -1)
                        {
                            Stop();
                            return;
                        }

                        var soundResponse = await _soundApi.GetAsync((int)id);

                        soundResponse.Match(
                            async s =>
                            {
                                var filename = string.Format("{0}.{1}", s.Id, "mp3");

                                if (!_cache.Contains(CacheType.AmbientSound, filename) ||
                                    Checksum.CreateHash(await _cache.GetData(CacheType.AmbientSound, filename)) != s.Checksum)
                                {
                                    var soundDataResponse = await _soundApi.GetDataAsync((int)id);
                                    soundDataResponse.Match(async x => await _cache.Add(CacheType.AmbientSound, filename, x.Data));
                                }

                                Play(_cache.GetPath(CacheType.AmbientSound, filename));
                            });
                    });
            }
        }

        private async void OnSoundEffectChanged(object? sender, EventArgs e)
        {
            if (_sessionData.CampaignId is int campaignId)
            {
                var response = await _activeSoundApi.GetAsync(campaignId);

                response.Match(
                    success =>
                    {
                        var id = success.EffectId;

                        if (id is int effectId && effectId != -1)
                        {
                            Play(effectId);
                        }
                    });
            }
        }
    }
}