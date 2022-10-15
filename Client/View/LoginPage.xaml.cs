using Client.Helper;
using Client.Services;
using Client.Services.API;
using DataTransfer.Login;
using System.Windows;
using System.Windows.Controls;

namespace Client.View
{
    public partial class LoginPage : Page
    {
        private readonly IPageNavigator _pageNavigator;
        private readonly ISessionData _sessionData;
        private readonly ILoginApi _loginApi;

        public LoginPage(IPageNavigator pageNavigator, ISessionData sessionData, ILoginApi loginApi)
        {
            InitializeComponent();
            _pageNavigator = pageNavigator;
            _sessionData = sessionData;
            _loginApi = loginApi;
            BackgroundImage.ImageSource = RandomBackgroundImage.GetImageFromResource();
        }

        private async void OnLogin(object sender, RoutedEventArgs e)
        {
            var response = await _loginApi.Get();

            if (response.Error is null)
            {
                switch (response.Data.State)
                {
                    case LoginState.Success:
                        _sessionData.UserId = response.Data.UserId;
                        _pageNavigator.OpenPage<CampaignSelectionPage>();
                        break;
                    case LoginState.UnknownEmail:
                        MessageBox.Show("Die angegebene Email-Adresse kann nicht gefunden werden.", "Login fehlgeschlagen", MessageBoxButton.OK);
                        break;
                    case LoginState.InvalidPassword:
                        MessageBox.Show("Das angegebene Passwort ist nicht korrekt.", "Login fehlgeschlagen", MessageBoxButton.OK);
                        break;
                }
            }
            else
            {
                MessageBox.Show(response.Error?.Message, "Netzwerkfehler", MessageBoxButton.OK);
            }
        }

        private void OnRegister(object sender, RoutedEventArgs e)
        {
            _pageNavigator.OpenPage<CampaignSelectionPage>();
        }
    }
}