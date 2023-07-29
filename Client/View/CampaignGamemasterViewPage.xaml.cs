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

        public CampaignGamemasterViewPage(IServiceProvider serviceProvider, IPageNavigator pageNavigator)
        {
            InitializeComponent();

            MapPresenter.Content = serviceProvider.GetRequiredService<MapControl>();
            MapOverviewPresenter.Content = serviceProvider.GetRequiredService<MapOverviewControl>();
            SoundPresenter.Content = serviceProvider.GetRequiredService<MusicLibrary>();
            PlayerPresenter.Content = serviceProvider.GetRequiredService<CharacterList>();
            NPCPresenter.Content = serviceProvider.GetService<MonsterControl>();

            _pageNavigator = pageNavigator;
        }

        private void OnExit(object sender, RoutedEventArgs e)
        {
            _pageNavigator.OpenPage<CampaignSelectionPage>();
        }
    }
}