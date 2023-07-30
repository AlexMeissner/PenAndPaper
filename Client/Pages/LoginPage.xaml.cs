using Client.Helper;
using Client.Services;
using Client.Services.API;
using Client.Windows;
using DataTransfer.Login;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static Client.Services.ServiceExtension;

namespace Client.Pages
{
    [TransistentService]
    public partial class LoginPage : Page
    {
        private readonly IPageNavigator _pageNavigator;
        private readonly ISessionData _sessionData;
        private readonly IAuthenticationApi _authenticationApi;

        public LoginPage(IPageNavigator pageNavigator, ISessionData sessionData, IAuthenticationApi loginApi)
        {
            InitializeComponent();
            _pageNavigator = pageNavigator;
            _sessionData = sessionData;
            _authenticationApi = loginApi;
            BackgroundImage.ImageSource = RandomBackgroundImage.GetImageFromResource();
        }

        private async void OnLogin(object sender, RoutedEventArgs e)
        {
            if (EmailBox.Text.Length > 0 && PasswordBox.Password.Length > 0)
            {
                UserCredentialsDto userCredentials = new(
                    Email: EmailBox.Text,
                    Username: string.Empty,
                    Password: Password.Encrypt(PasswordBox.Password));

                var response = await _authenticationApi.LoginAsync(userCredentials);

                response.Match(
                    success =>
                    {
                        _sessionData.UserId = success.UserId;
                        _pageNavigator.OpenPage<CampaignSelectionPage>();
                    },
                    failure =>
                    {
                        MessageBoxUtility.Show(failure);
                    });
            }
        }

        private void OnRegister(object sender, RoutedEventArgs e)
        {
            _pageNavigator.OpenPage<RegisterPage>();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.End || e.Key == Key.Return)
            {
                OnLogin(sender, e);
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(EmailBox);
        }
    }
}