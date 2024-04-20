using Client.Controls;
using Client.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using static Client.Services.ServiceExtension;

namespace Client.Pages
{
    [TransistentService]
    public partial class CampaignPlayerViewPage : Page
    {
        private readonly IPageNavigator _pageNavigator;

        public IPopupPage PopupPage { get; }

        public CampaignPlayerViewPage(IServiceProvider serviceProvider, IPopupPage popupPage, IPageNavigator pageNavigator)
        {
            _pageNavigator = pageNavigator;
            PopupPage = popupPage;

            InitializeComponent();

            MapPresenter.Content = serviceProvider.GetRequiredService<Map>();            
        }

        private void OnOpenSettings(object sender, RoutedEventArgs e)
        {
            PopupPage.Open<SettingsView>("Einstellungen");
        }

        private void OnExit(object sender, RoutedEventArgs e)
        {
            _pageNavigator.OpenPage<CampaignSelectionPage>();
        }

        private void OnNavigating(object sender, NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Forward || e.NavigationMode == NavigationMode.Back)
            {
                e.Cancel = true;
            }
        }
    }
}