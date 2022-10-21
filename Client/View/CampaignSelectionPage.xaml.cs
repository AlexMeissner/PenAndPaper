using Client.Helper;
using Client.Services;
using Client.Services.API;
using DataTransfer.CampaignSelection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Client.View
{
    public partial class CampaignSelectionPage : Page
    {
        private readonly IPageNavigator _pageNavigator;
        private readonly ISessionData _sessionData;
        private readonly ICampaignOverviewApi _campaignOverviewApi;

        public CampaignSelectionPage(IPageNavigator pageNavigator, ISessionData sessionData, ICampaignOverviewApi campaignOverviewApi)
        {
            InitializeComponent();
            _pageNavigator = pageNavigator;
            _sessionData = sessionData;
            _campaignOverviewApi = campaignOverviewApi;
            BackgroundImage.ImageSource = RandomBackgroundImage.GetImageFromResource();
        }

        private void OnCreateCampaign(object sender, RoutedEventArgs e)
        {
            _pageNavigator.OpenPage<CampaignCreationPage>();
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            var response = await _campaignOverviewApi.GetAsync(_sessionData.UserId ?? 0); // TODO: why does '!' not work?

            if (response.Error is null)
            {
                DataContext = response.Data;
            }
            else
            {
                throw new System.NullReferenceException(response.Error.Message);
            }
        }

        private void OnEnterCampaign(object sender, RoutedEventArgs e)
        {
            if (CampaignListView.SelectedItem is CampaignOverviewItemDto selectedCampaign)
            {
                _sessionData.CampaignId = selectedCampaign.Id;

                if (selectedCampaign.IsGamemaster)
                {
                    _pageNavigator.OpenPage<CampaignGamemasterViewPage>();
                }
                else
                {
                    _pageNavigator.OpenPage<CampaignPlayerViewPage>();
                }
            }
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                OnEnterCampaign(sender, e);
            }
        }
    }
}