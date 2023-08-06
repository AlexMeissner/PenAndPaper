using Client.Commands;
using Client.Services;
using Client.Services.API;
using Client.Windows;
using DataTransfer.Sound;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Effects;
using static Client.Services.ServiceExtension;

namespace Client.ViewModels
{
    [TransistentService]
    public class MusicLibraryViewModel : BaseViewModel
    {
        public ObservableCollection<SoundOverviewItemDto> AmbientSounds { get; set; } = new();
        public ObservableCollection<SoundOverviewItemDto> Effects { get; set; } = new();

        public string AmbientFilter { get; set; } = string.Empty;
        public string EffectsFilter { get; set; } = string.Empty;

        public ICommand PlayAmbientCommand { get; init; }
        public ICommand PlayPlaylistCommand { get; init; }
        public ICommand StopPlaylistCommand { get; init; }
        public ICommand PlayEffectCommand { get; init; }

        private readonly ISessionData _sessionData;
        private readonly ISoundApi _soundApi;
        private readonly IActiveSoundApi _activeSoundApi;
        private readonly IAudioPlayer _audioPlayer;

        private int PlaylistIndex = 0;
        private IList<SoundOverviewItemDto> Playlist = new List<SoundOverviewItemDto>();

        public MusicLibraryViewModel(ISessionData sessionData, ISoundApi soundApi, IAudioPlayer audioPlayer, IActiveSoundApi activeSoundApi)
        {
            _sessionData = sessionData;
            _soundApi = soundApi;
            _activeSoundApi = activeSoundApi;
            _audioPlayer = audioPlayer;

            PlayAmbientCommand = new AsyncCommand<SoundOverviewItemDto>(OnPlayAmbient);
            PlayPlaylistCommand = new AsyncCommand(OnPlayAll);
            StopPlaylistCommand = new AsyncCommand(OnStopAmbient);
            PlayEffectCommand = new AsyncCommand<SoundOverviewItemDto>(OnPlayEffect);

            _audioPlayer.Finished += OnAmbientFinished;
        }

        public async Task AddSound(SoundCreationDto payload)
        {
            var response = await _soundApi.PostAsync(payload);

            if (response.Succeded)
            {
                await LoadOverview();
            }
        }

        public async void OnAmbientFinished(object? sender, EventArgs e)
        {
            if (Playlist.Count > 0)
            {
                PlaylistIndex = (PlaylistIndex + 1) % Playlist.Count;

                var payload = new ActiveSoundDto(
                    CampaignId: _sessionData.CampaignId,
                    AmbientId: Playlist[PlaylistIndex].Id,
                    EffectId: null
                );

                await _activeSoundApi.PutAsync(payload);
            }
        }

        private async Task OnPlayAmbient(SoundOverviewItemDto sound)
        {
            Playlist.Clear();

            var payload = new ActiveSoundDto(
                CampaignId: _sessionData.CampaignId,
                AmbientId: sound.Id,
                EffectId: null
            );

            await _activeSoundApi.PutAsync(payload);
        }

        private async Task OnPlayEffect(SoundOverviewItemDto sound)
        {
            var payload = new ActiveSoundDto(
                CampaignId: _sessionData.CampaignId,
                AmbientId: null,
                EffectId: sound.Id
            );

            await _activeSoundApi.PutAsync(payload);
        }

        private async Task OnStopAmbient()
        {
            var payload = new ActiveSoundDto(
                CampaignId: _sessionData.CampaignId,
                AmbientId: -1,
                EffectId: null
            );

            await _activeSoundApi.PutAsync(payload);
        }

        public async Task LoadOverview()
        {
            var response = await _soundApi.GetOverviewAsync();

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

        public bool FilterAmbient(object item)
        {
            if (item is SoundOverviewItemDto soundOverviewItem)
            {
                if (string.IsNullOrEmpty(AmbientFilter) ||
                    soundOverviewItem.Tags.Any(x => x.ToUpper().Contains(AmbientFilter.ToUpper())))
                {
                    return true;
                }

                return soundOverviewItem.Name.ToUpper().Contains(AmbientFilter.ToUpper());
            }

            return false;
        }

        public bool FilterEffects(object item)
        {
            if (item is SoundOverviewItemDto soundOverviewItem)
            {
                if (string.IsNullOrEmpty(EffectsFilter) ||
                    soundOverviewItem.Tags.Any(x => x.ToUpper().Contains(EffectsFilter.ToUpper())))
                {
                    return true;
                }

                return soundOverviewItem.Name.ToUpper().Contains(EffectsFilter.ToUpper());
            }

            return false;
        }

        public async Task OnPlayAll()
        {
            PlaylistIndex = 0;

            var filteredAmbientSounds = ((CollectionView)CollectionViewSource.GetDefaultView(AmbientSounds)).Cast<SoundOverviewItemDto>();
            var random = new Random();
            Playlist = filteredAmbientSounds.Select(x => new { key = random.Next(), x }).OrderBy(y => y.key).Select(z => z.x).ToList();

            var payload = new ActiveSoundDto(

                CampaignId: _sessionData.CampaignId,
                AmbientId: Playlist[PlaylistIndex].Id,
                EffectId: null
            );

            await _activeSoundApi.PutAsync(payload);
        }
    }
}
