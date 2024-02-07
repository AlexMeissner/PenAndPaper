using Client.Services;
using Client.ViewModels;
using Client.Windows;
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
    }
}