using Client.Services.API;
using Client.View;
using DataTransfer.Sound;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Client.Controls
{
    public partial class GamemasterMusicControl : UserControl
    {
        public ICollection<SoundOverviewItemDto> AmbientSounds { get; set; } = new ObservableCollection<SoundOverviewItemDto>();
        public ICollection<SoundOverviewItemDto> Effects { get; set; } = new ObservableCollection<SoundOverviewItemDto>();

        private readonly ISoundApi SoundApi;

        public GamemasterMusicControl(ISoundApi soundApi)
        {
            SoundApi = soundApi;

            InitializeComponent();

            var ambientCollectionView = (CollectionView)CollectionViewSource.GetDefaultView(AmbientControl.ItemsSource);
            ambientCollectionView.Filter = AmbientFilter;

            var effectsCollectionView = (CollectionView)CollectionViewSource.GetDefaultView(EffectsControl.ItemsSource);
            effectsCollectionView.Filter = EffectsFilter;
        }

        private void OnClicked(object sender, RoutedEventArgs e)
        {
            if (sender is SoundDto sound)
            {

            }
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            await LoadOverview();
        }

        private async void OnAddSound(object sender, RoutedEventArgs e)
        {
            var window = new SoundCreationWindow();

            if (window.ShowDialog() is true)
            {
                var response = await SoundApi.PostAsync(window.CreationData);

                if (response.Error is null)
                {
                    await LoadOverview();
                }
                else
                {
                    MessageBox.Show(response.Error.Message, "Fehler bei Sounderstellung", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async Task LoadOverview()
        {
            var response = await SoundApi.GetOverviewAsync();

            if (response.Error is null)
            {
                AmbientSounds.Clear();
                Effects.Clear();

                foreach (var item in response.Data.Items)
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
            }
            else
            {
                MessageBox.Show(response.Error.Message, "Fehler bei Sounddateiabfrage", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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

        private void OnPlayAll(object sender, RoutedEventArgs e)
        {

        }
    }
}