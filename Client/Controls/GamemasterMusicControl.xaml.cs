﻿using Client.Services;
using Client.Services.API;
using Client.View;
using DataTransfer.Sound;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using static Client.Services.ServiceExtension;

namespace Client.Controls
{
    [TransistentService]
    public partial class GamemasterMusicControl : UserControl
    {
        public ICollection<SoundOverviewItemDto> AmbientSounds { get; set; } = new ObservableCollection<SoundOverviewItemDto>();
        public ICollection<SoundOverviewItemDto> Effects { get; set; } = new ObservableCollection<SoundOverviewItemDto>();

        private readonly ISessionData SessionData;
        private readonly ISoundApi SoundApi;
        private readonly IActiveSoundApi ActiveSoundApi;
        private readonly IAudioPlayer AudioPlayer;

        private int PlaylistIndex = 0;
        private IList<SoundOverviewItemDto> Playlist = new List<SoundOverviewItemDto>();

        public GamemasterMusicControl(ISessionData sessionData, ISoundApi soundApi, IAudioPlayer audioPlayer, IActiveSoundApi activeSoundApi)
        {
            SessionData = sessionData;
            SoundApi = soundApi;
            ActiveSoundApi = activeSoundApi;
            AudioPlayer = audioPlayer;

            InitializeComponent();

            var ambientCollectionView = (CollectionView)CollectionViewSource.GetDefaultView(AmbientControl.ItemsSource);
            ambientCollectionView.Filter = AmbientFilter;

            var effectsCollectionView = (CollectionView)CollectionViewSource.GetDefaultView(EffectsControl.ItemsSource);
            effectsCollectionView.Filter = EffectsFilter;

            AudioPlayer.Finished += OnAmbientFinished;
        }

        private async void OnAmbientFinished(object? sender, EventArgs e)
        {
            if (Playlist.Count > 0 && SessionData.CampaignId is int campaignId)
            {
                PlaylistIndex = (PlaylistIndex + 1) % Playlist.Count;

                var payload = new ActiveSoundDto()
                {
                    CampaignId = campaignId,
                    AmbientId = Playlist[PlaylistIndex].Id
                };
                await ActiveSoundApi.PutAsync(payload);
            }
        }

        private async void OnPlayAmbient(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is SoundOverviewItemDto sound && SessionData.CampaignId is int campaignId)
            {
                Playlist.Clear();

                var payload = new ActiveSoundDto()
                {
                    CampaignId = campaignId,
                    AmbientId = sound.Id
                };
                await ActiveSoundApi.PutAsync(payload);
            }
        }

        private async void OnPlayEffect(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is SoundOverviewItemDto sound && SessionData.CampaignId is int campaignId)
            {
                var payload = new ActiveSoundDto()
                {
                    CampaignId = campaignId,
                    EffectId = sound.Id
                };
                await ActiveSoundApi.PutAsync(payload);
            }
        }

        private async void OnStopAmbient(object sender, RoutedEventArgs e)
        {
            if (SessionData.CampaignId is int campaignId)
            {
                var payload = new ActiveSoundDto()
                {
                    CampaignId = campaignId,
                    AmbientId = -1
                };
                await ActiveSoundApi.PutAsync(payload);
            }
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            await LoadOverview();
        }

        private async void OnAddAmbientSound(object sender, RoutedEventArgs e)
        {
            await OnAddSound(SoundType.Ambient);
        }

        private async void OnAddSoundEffect(object sender, RoutedEventArgs e)
        {
            await OnAddSound(SoundType.Effect);
        }

        private async Task OnAddSound(SoundType type)
        {
            var window = new SoundCreationWindow(type);

            if (window.ShowDialog() is true)
            {
                var response = await SoundApi.PostAsync(window.CreationData);

                if (response.Succeded)
                {
                    await LoadOverview();
                }
                else
                {
                    MessageBoxUtility.Show(response.StatusCode);
                }
            }
        }

        private async Task LoadOverview()
        {
            var response = await SoundApi.GetOverviewAsync();

            response.Match(
                success =>
                {
                    AmbientSounds.Clear();
                    Effects.Clear();

                    foreach (var item in success.Items)
                    {
                        if (item.Type == SoundType.Ambient)
                        {
                            AmbientSounds.Add(item);
                        }
                        else if (item.Type == SoundType.Effect)
                        {
                            Effects.Add(item);
                        }
                    }
                },
                failure =>
                {
                    MessageBoxUtility.Show(failure);
                });
        }

        private bool AmbientFilter(object item)
        {
            if (item is SoundOverviewItemDto soundOverviewItem)
            {
                if (string.IsNullOrEmpty(AmbientFilterTextBox.Text) ||
                    soundOverviewItem.Tags.Any(x => x.ToUpper().Contains(AmbientFilterTextBox.Text.ToUpper())))
                {
                    return true;
                }

                return soundOverviewItem.Name.ToUpper().Contains(AmbientFilterTextBox.Text.ToUpper());
            }

            return false;
        }

        private bool EffectsFilter(object item)
        {
            if (item is SoundOverviewItemDto soundOverviewItem)
            {
                if (string.IsNullOrEmpty(EffectsFilterTextBox.Text) ||
                    soundOverviewItem.Tags.Any(x => x.ToUpper().Contains(EffectsFilterTextBox.Text.ToUpper())))
                {
                    return true;
                }

                return soundOverviewItem.Name.ToUpper().Contains(EffectsFilterTextBox.Text.ToUpper());
            }

            return false;
        }

        private void OnAmbientFilterChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(AmbientControl.ItemsSource).Refresh();
        }

        private void OnEffectsFilterChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(EffectsControl.ItemsSource).Refresh();
        }

        private async void OnPlayAll(object sender, RoutedEventArgs e)
        {
            PlaylistIndex = 0;

            var filteredAmbientSounds = ((CollectionView)CollectionViewSource.GetDefaultView(AmbientControl.ItemsSource)).Cast<SoundOverviewItemDto>();
            var random = new Random();
            Playlist = filteredAmbientSounds.Select(x => new { key = random.Next(), x }).OrderBy(y => y.key).Select(z => z.x).ToList();

            if (SessionData.CampaignId is int campaignId)
            {
                var payload = new ActiveSoundDto()
                {
                    CampaignId = campaignId,
                    AmbientId = Playlist[PlaylistIndex].Id
                };
                await ActiveSoundApi.PutAsync(payload);
            }
        }
    }
}