using Client.Controls;
using Client.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using System.Windows.Controls;
using static Client.Services.ServiceExtension;

namespace Client.View
{
    [TransistentService]
    public partial class CampaignGamemasterViewPage : Page
    {
        private readonly IPageNavigator _pageNavigator;
        private readonly ISessionData _sessionData;
        private readonly ICampaignUpdates _campaignUpdates;

        public CampaignGamemasterViewPage(IServiceProvider serviceProvider, IPageNavigator pageNavigator, ISessionData sessionData, ICampaignUpdates campaignUpdates)
        {
            InitializeComponent();

            MapPresenter.Content = serviceProvider.GetRequiredService<MapControl>();
            MapOverviewPresenter.Content = serviceProvider.GetRequiredService<MapOverviewControl>();
            SoundPresenter.Content = serviceProvider.GetRequiredService<GamemasterMusicControl>();
            PlayerPresenter.Content = serviceProvider.GetRequiredService<CharacterControl>();
            NPCPresenter.Content = serviceProvider.GetService<MonsterControl>();

            _pageNavigator = pageNavigator;
            _sessionData = sessionData;
            _campaignUpdates = campaignUpdates;
        }

        private void OnExit(object sender, RoutedEventArgs e)
        {
            _sessionData.CampaignId = null;
            _pageNavigator.OpenPage<CampaignSelectionPage>();
        }
    }
}