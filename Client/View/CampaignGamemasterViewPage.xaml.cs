using Client.Controls;
using Client.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Client.View
{
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