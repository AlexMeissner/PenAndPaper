using Client.Commands;
using Client.Pages;
using Client.Services;
using System.Windows.Input;
using static Client.Services.ServiceExtension;

namespace Client.ViewModels
{
    [TransistentService]
    public class CampaignSelectionViewModel
    {
        private readonly IPageNavigator _pageNavigator;
        private readonly ISessionData _sessionData;

        public ICommand OpenAdminViewCommand { get; }

        public CampaignSelectionViewModel(IPageNavigator pageNavigator, ISessionData sessionData)
        {
            _pageNavigator = pageNavigator;
            _sessionData = sessionData;

            OpenAdminViewCommand = new RelayCommand(OpenAdminView);
        }

        private void OpenAdminView()
        {
            _pageNavigator.OpenPage<AdminView>();
        }
    }
}
