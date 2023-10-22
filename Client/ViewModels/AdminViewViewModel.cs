using Client.Commands;
using Client.Pages;
using Client.Services;
using System.Windows.Input;
using static Client.Services.ServiceExtension;

namespace Client.ViewModels
{
    [TransistentService]
    public class AdminViewViewModel
    {
        private readonly IPageNavigator _pageNavigator;

        public ICommand CancelCommand { get; }
        public ICommand SaveCommand { get; }

        public AdminViewViewModel(IPageNavigator pageNavigator)
        {
            _pageNavigator = pageNavigator;
            CancelCommand = new RelayCommand(OnCancel);
            SaveCommand = new RelayCommand(OnSave);
        }

        private void OnCancel()
        {
            _pageNavigator.OpenPage<CampaignSelectionPage>();
        }

        private void OnSave()
        {
            _pageNavigator.OpenPage<CampaignSelectionPage>();
        }
    }
}
