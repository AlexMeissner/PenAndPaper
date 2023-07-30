using Client.Controls;
using Client.Services;
using System.Windows;
using System.Windows.Controls;
using static Client.Services.ServiceExtension;

namespace Client.Pages
{
    [TransistentService]
    public partial class CampaignGamemasterViewPage : Page
    {
        private readonly IPageNavigator _pageNavigator;

        public CampaignGamemasterViewPage(IControlProvider controlProvider, IPageNavigator pageNavigator)
        {
            InitializeComponent();

            MapPresenter.Content = controlProvider.Get<Map>();
            MapOverviewPresenter.Content = controlProvider.Get<MapOverview>();
            SoundPresenter.Content = controlProvider.Get<MusicLibrary>();
            PlayerPresenter.Content = controlProvider.Get<CharacterList>();
            NPCPresenter.Content = controlProvider.Get<MonsterControl>();
            ScriptPresenter.Content = controlProvider.Get<Script>();

            _pageNavigator = pageNavigator;
        }

        private void OnExit(object sender, RoutedEventArgs e)
        {
            _pageNavigator.OpenPage<CampaignSelectionPage>();
        }
    }
}