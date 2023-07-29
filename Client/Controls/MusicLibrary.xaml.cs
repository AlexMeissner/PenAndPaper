using Client.Services;
using Client.View;
using Client.ViewModels;
using DataTransfer.Sound;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using static Client.Services.ServiceExtension;

namespace Client.Controls
{
    [TransistentService]
    public partial class MusicLibrary : UserControl
    {
        public MusicLibraryViewModel ViewModel => (MusicLibraryViewModel)DataContext;

        public MusicLibrary(IViewModelProvider viewModelProvider)
        {
            DataContext = viewModelProvider.Get<MusicLibraryViewModel>();
            InitializeComponent();

            var ambientCollectionView = (CollectionView)CollectionViewSource.GetDefaultView(AmbientControl.ItemsSource);
            ambientCollectionView.Filter = ViewModel.FilterAmbient;

            var effectsCollectionView = (CollectionView)CollectionViewSource.GetDefaultView(EffectsControl.ItemsSource);
            effectsCollectionView.Filter = ViewModel.FilterEffects;
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadOverview();
        }

        private void OnAmbientFilterChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(AmbientControl.ItemsSource).Refresh();
        }

        private void OnEffectsFilterChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(EffectsControl.ItemsSource).Refresh();
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
                await ViewModel.AddSound(window.CreationData);
            }
        }
    }
}