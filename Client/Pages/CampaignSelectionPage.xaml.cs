using Client.Helper;
using Client.Services;
using Client.Services.API;
using Client.ViewModels;
using Client.Windows;
using DataTransfer.Campaign;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static Client.Services.ServiceExtension;

namespace Client.Pages
{
    [TransistentService]
    public partial class CampaignSelectionPage : Page
    {
        private readonly IPageNavigator _pageNavigator;
        private readonly ISessionData _sessionData;
        private readonly ICampaignOverviewApi _campaignOverviewApi;
        private readonly IUpdateNotifier _updateNotifier;

        public CampaignSelectionViewModel ViewModel { get; }

        public CampaignSelectionPage(CampaignSelectionViewModel viewModel, IPageNavigator pageNavigator, ISessionData sessionData, ICampaignOverviewApi campaignOverviewApi, IUpdateNotifier updateNotifier)
        {
            ViewModel = viewModel;

            InitializeComponent();

            _pageNavigator = pageNavigator;
            _sessionData = sessionData;
            _campaignOverviewApi = campaignOverviewApi;
            _updateNotifier = updateNotifier;
            BackgroundImage.ImageSource = RandomBackgroundImage.GetImageFromResource();
        }

        private void OnCreateCampaign(object sender, RoutedEventArgs e)
        {
            _pageNavigator.OpenPage<CampaignCreationPage>();
        }

        private void OnEditCampaign(object sender, RoutedEventArgs e)
        {
            _pageNavigator.OpenPage<CampaignCreationPage>();

            if (_pageNavigator.CurrentPage is CampaignCreationPage page &&
                e.Source is Button button &&
                button.DataContext is CampaignOverviewItemDto campaign)
            {
                page.CampaignId = campaign.Id;
            }
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            var response = await _campaignOverviewApi.GetAsync(_sessionData.UserId);

            response.Match(
                success => DataContext = success,
                failure => MessageBoxUtility.Show(failure));
        }

        private async void OnEnterCampaign(object sender, RoutedEventArgs e)
        {
            if (CampaignListView.SelectedItem is CampaignOverviewItemDto selectedCampaign)
            {
                _sessionData.CampaignId = selectedCampaign.Id;
                await _updateNotifier.SetCampaignAsync(selectedCampaign.Id);

                if (selectedCampaign.IsGamemaster)
                {
                    _pageNavigator.OpenPage<GamemasterView>();
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