using Client.Services;
using System.Windows;
using System.Windows.Controls;

namespace Client.View
{
    public partial class CampaignGamemasterViewPage : Page
    {
        private readonly IPageNavigator _pageNavigator;
        private readonly ISessionData _sessionData;

        public CampaignGamemasterViewPage(IPageNavigator pageNavigator, ISessionData sessionData)
        {
            InitializeComponent();
            _pageNavigator = pageNavigator;
            _sessionData = sessionData;
        }

        private void OnExit(object sender, RoutedEventArgs e)
        {
            _sessionData.CampaignId = null;
            _pageNavigator.OpenPage<CampaignSelectionPage>();
        }
    }
}