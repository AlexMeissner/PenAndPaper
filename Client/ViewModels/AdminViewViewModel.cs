using Client.Commands;
using Client.Pages;
using Client.Services;
using Client.Services.API;
using DataTransfer.Settings;
using DataTransfer.Sound;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using static Client.Services.ServiceExtension;

namespace Client.ViewModels
{
    public class SoundCreationViewModel : BaseViewModel
    {
        public string Name { get; set; } = string.Empty;
        public SoundType Type { get; set; }
        public static IEnumerable<SoundType> SoundTypeValues
        {
            get
            {
                return Enum.GetValues(typeof(SoundType)).Cast<SoundType>();
            }
        }
        public string Tag { get; set; } = string.Empty;
        public ObservableCollection<string> Tags { get; set; } = [];
        public byte[] Data { get; set; } = default!;

        public ICommand SelectFileCommand { get; }
        public ICommand AddTagCommand { get; }
        public ICommand AddSoundCommand { get; }

        private readonly ISoundApi _soundApi;

        public SoundCreationViewModel(ISoundApi soundApi)
        {
            _soundApi = soundApi;
            SelectFileCommand = new AsyncCommand(SelectFile);
            AddTagCommand = new RelayCommand(AddTag);
            AddSoundCommand = new AsyncCommand(AddSound);
        }

        private async Task SelectFile()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "MP3 (*.mp3)|*.mp3"
            };

            if (openFileDialog.ShowDialog() is true)
            {
                Data = await File.ReadAllBytesAsync(openFileDialog.FileName);
            }
        }

        private void AddTag()
        {
            Tags.Add(Tag);
            Tag = string.Empty;
        }

        private async Task AddSound()
        {
            var payload = new SoundCreationDto(Name, Type, Tags, Data);
            await _soundApi.PostAsync(payload);

            Name = string.Empty;
            Tag = string.Empty;
            Tags.Clear();
            Data = default!;
        }
    }

    public class DiceOptionsViewModel : BaseViewModel
    {
        public SoundOverviewItemDto? SuccessSound { get; set; }
        public SoundOverviewItemDto? FailSound { get; set; }
        public SoundOverviewItemDto? CritSuccessSound { get; set; }
        public SoundOverviewItemDto? CritFailSound { get; set; }
        public ObservableCollection<SoundOverviewItemDto> Sounds { get; set; } = [];

        public ICommand PlaySuccessCommand { get; }
        public ICommand PlayFailCommand { get; }
        public ICommand PlayCritSuccessCommand { get; }
        public ICommand PlayCritFailCommand { get; }
        public ICommand SaveCommand { get; }

        private readonly ISoundApi _soundApi;
        private readonly ISettingsApi _settingsApi;
        private readonly IAudioPlayer _audioPlayer;

        public DiceOptionsViewModel(ISoundApi soundApi, ISettingsApi settingsApi, IAudioPlayer audioPlayer)
        {
            _soundApi = soundApi;
            _settingsApi = settingsApi;
            _audioPlayer = audioPlayer;
            PlaySuccessCommand = new RelayCommand(PlayPositive);
            PlayFailCommand = new RelayCommand(PlayFail);
            PlayCritSuccessCommand = new RelayCommand(PlayCritSuccess);
            PlayCritFailCommand = new RelayCommand(PlayCritFail);
            SaveCommand = new AsyncCommand(Save);

            // ToDo: Code Small -> Async Code made sync because CTOR
            var getOverviewTask = _soundApi.GetOverviewAsync();
            getOverviewTask.Wait();
            getOverviewTask.Result.Match(
                success =>
                {
                    var soundEffects = success.Items.Where(x => x.Type == SoundType.Effect);

                    Sounds.Clear();

                    foreach (var soundEffect in soundEffects)
                    {
                        Sounds.Add(soundEffect);
                    }
                },
                fail => { });

            var settingsTask = _settingsApi.GetAsync();
            settingsTask.Wait();
            settingsTask.Result.Match(
                success =>
                {
                    SuccessSound = Sounds.FirstOrDefault(sound => sound.Id == success.DiceSuccessSoundId);
                    FailSound = Sounds.FirstOrDefault(sound => sound.Id == success.DiceFailSoundId);
                    CritSuccessSound = Sounds.FirstOrDefault(sound => sound.Id == success.DiceCritSuccessSoundId);
                    CritFailSound = Sounds.FirstOrDefault(sound => sound.Id == success.DiceCritFailSoundId);
                },
                failure => { });
        }

        private void PlayPositive()
        {
            if (SuccessSound is not null)
            {
                _audioPlayer.Play(SuccessSound.Id);
            }
        }

        private void PlayFail()
        {
            if (FailSound is not null)
            {
                _audioPlayer.Play(FailSound.Id);
            }
        }

        private void PlayCritSuccess()
        {
            if (CritSuccessSound is not null)
            {
                _audioPlayer.Play(CritSuccessSound.Id);
            }
        }

        private void PlayCritFail()
        {
            if (CritFailSound is not null)
            {
                _audioPlayer.Play(CritFailSound.Id);
            }
        }

        private async Task Save()
        {
            var settings = new SettingsDto(SuccessSound?.Id, FailSound?.Id, CritSuccessSound?.Id, CritFailSound?.Id);
            var settingsTask = await _settingsApi.GetAsync();

            settingsTask.Match(
                success =>
                {
                    _settingsApi.PutAsync(settings);
                },
                failure =>
                {
                    _settingsApi.PostAsync(settings);
                });
        }
    }

    [TransistentService]
    public class AdminViewViewModel : BaseViewModel
    {
        private readonly IPageNavigator _pageNavigator;

        public SoundCreationViewModel SoundCreation { get; set; }
        public DiceOptionsViewModel DiceOptions { get; set; }

        public ICommand CloseCommand { get; }

        public AdminViewViewModel(IPageNavigator pageNavigator, ISoundApi soundApi, ISettingsApi settingsApi, IAudioPlayer audioPlayer)
        {
            SoundCreation = new(soundApi);
            DiceOptions = new(soundApi, settingsApi, audioPlayer);
            _pageNavigator = pageNavigator;
            CloseCommand = new RelayCommand(OnClose);
        }

        private void OnClose()
        {
            _pageNavigator.OpenPage<CampaignSelectionPage>();
        }
    }
}
