using Client.Helper;
using Client.Services;
using Client.Services.API;
using DataTransfer.CampaignCreation;
using DataTransfer.User;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static Client.Services.ServiceExtension;

namespace Client.View
{
    [TransistentService]
    public partial class CampaignCreationPage : Page
    {
        private readonly int _campaignId;
        private readonly IPageNavigator _pageNavigator;
        private readonly ICampaignCreationApi _campaignCreationApi;
        private readonly IUserApi _userApi;
        private readonly ISessionData _sessionData;

        public CampaignCreationPage(IPageNavigator pageNavigator, ICampaignCreationApi campaignCreationApi, IUserApi userApi, ISessionData sessionData)
            : this(pageNavigator, campaignCreationApi, userApi, sessionData, -1)
        {
        }

        public CampaignCreationPage(IPageNavigator pageNavigator, ICampaignCreationApi campaignCreationApi, IUserApi userApi, ISessionData sessionData, int campaignId)
        {
            InitializeComponent();
            _campaignId = campaignId;
            _pageNavigator = pageNavigator;
            _campaignCreationApi = campaignCreationApi;
            _userApi = userApi;
            _sessionData = sessionData;
            BackgroundImage.ImageSource = RandomBackgroundImage.GetImageFromResource();
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed &&
                sender is ListView listView &&
                listView.SelectedItems.Count > 0)
            {
                DragDrop.DoDragDrop(this, new DataObject(DataFormats.FileDrop, listView), DragDropEffects.Move);
            }

        }

        private void OnDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(DataFormats.FileDrop) is ListView source &&
                sender is ListView target &&
                source != target)
            {
                var selectedItem = (UsersDto)source.SelectedItem;
                var targetCollection = (ICollection<UsersDto>)target.ItemsSource;
                var sourceCollection = (ICollection<UsersDto>)source.ItemsSource;
                targetCollection.Add(selectedItem);
                sourceCollection.Remove(selectedItem);
                target.Items.Refresh();
                source.Items.Refresh();
                e.Handled = true;
            }

        }

        private void OnAbort(object sender, RoutedEventArgs e)
        {
            _pageNavigator.OpenPage<CampaignSelectionPage>();
        }

        private async void OnSave(object sender, RoutedEventArgs e)
        {
            var response = await _campaignCreationApi.PostAsync((DataContext as CampaignCreationDto)!);

            if (response.Succeded)
            {
                _pageNavigator.OpenPage<CampaignSelectionPage>();
            }
            else
            {
                MessageBoxUtility.Show(response.StatusCode);
            }
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            var response = await _campaignCreationApi.GetAsync(_campaignId);

            response.Match(
                async success =>
                {
                    if (success.Gamemaster is null)
                    {
                        var me = await _userApi.GetAsync(_sessionData.UserId);

                        me.Match(
                            s => success.Gamemaster = s,
                            f => MessageBoxUtility.Show(f));
                    }

                    DataContext = success;
                },
                failure =>
                {
                    MessageBoxUtility.Show(failure);
                });
        }
    }
}