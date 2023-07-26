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
    public partial class CampaignPlayerViewPage : Page
    {
        private readonly IPageNavigator _pageNavigator;
        private readonly ISessionData _sessionData;

        public CampaignPlayerViewPage(IServiceProvider serviceProvider, IPageNavigator pageNavigator, ISessionData sessionData)
        {
            InitializeComponent();

            MapPresenter.Content = serviceProvider.GetRequiredService<MapControl>();

            _pageNavigator = pageNavigator;
            _sessionData = sessionData;
        }

        private void OnExit(object sender, RoutedEventArgs e)
        {
            _pageNavigator.OpenPage<CampaignSelectionPage>();
        }
    }
}