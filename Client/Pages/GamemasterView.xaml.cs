using Client.Controls;
using Client.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using static Client.Services.ServiceExtension;

namespace Client.Pages
{
    [TransistentService]
    public partial class GamemasterView : Page
    {
        private readonly IPageNavigator _pageNavigator;
        private readonly IAudioPlayer _audioPlayer;

        public IPopupPage PopupPage { get; }

        public GamemasterView(IControlProvider controlProvider, IPageNavigator pageNavigator, IPopupPage popupPage, IAudioPlayer audioPlayer)
        {
            _audioPlayer = audioPlayer;
            _pageNavigator = pageNavigator;
            PopupPage = popupPage;

            InitializeComponent();

            MapPresenter.Content = controlProvider.Get<Map>();
            MapOverviewPresenter.Content = controlProvider.Get<MapOverview>();
            SoundPresenter.Content = controlProvider.Get<MusicLibrary>();
            PlayerPresenter.Content = controlProvider.Get<CharacterList>();
            MonsterListPresenter.Content = controlProvider.Get<MonsterControl>();
            ScriptPresenter.Content = controlProvider.Get<Script>();
        }

        private void OnExit(object sender, RoutedEventArgs e)
        {
            _audioPlayer.Stop();
            _pageNavigator.OpenPage<CampaignSelectionPage>();
        }

        private void OnNavigating(object sender, NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Forward || e.NavigationMode == NavigationMode.Back)
            {
                e.Cancel = true;
            }
        }

        private void OnOpenSettings(object sender, RoutedEventArgs e)
        {
            PopupPage.Open<SettingsView>("Einstellungen");
        }
    }
}