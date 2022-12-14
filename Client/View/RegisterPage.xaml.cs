using Client.Helper;
using Client.Services.API;
using Client.Services;
using System.Windows;
using System.Windows.Controls;
using DataTransfer.Login;

namespace Client.View
{
    public partial class RegisterPage : Page
    {
        private readonly IPageNavigator _pageNavigator;
        private readonly ISessionData _sessionData;
        private readonly IAuthenticationApi _authenticationApi;

        public RegisterPage(IPageNavigator pageNavigator, ISessionData sessionData, IAuthenticationApi loginApi)
        {
            InitializeComponent();
            _pageNavigator = pageNavigator;
            _sessionData = sessionData;
            _authenticationApi = loginApi;
            BackgroundImage.ImageSource = RandomBackgroundImage.GetImageFromResource();
        }

        private void OnAbort(object sender, RoutedEventArgs e)
        {
            _pageNavigator.OpenPage<LoginPage>();
        }

        private async void OnRegister(object sender, RoutedEventArgs e)
        {
            if (EmailBox.Text.Length > 0 &&
                Email.IsValid(EmailBox.Text) &&
                UsernameBox.Text.Length > 0 &&
                PasswordBox.Password.Length > 0 &&
                PasswordBox.Password == PasswordBoxRepeat.Password)
            {
                UserCredentialsDto userCredentials = new(Email: EmailBox.Text, Username: UsernameBox.Text, Password: Password.Encrypt(PasswordBox.Password));
                var response = await _authenticationApi.RegisterAsync(userCredentials);

                if (response.Error is null)
                {
                    _sessionData.UserId = response.Data.UserId;
                    _pageNavigator.OpenPage<CampaignSelectionPage>();
                }
                else
                {
                    MessageBox.Show(response.Error.Message, "Authentifizierungsfehler", MessageBoxButton.OK);
                }
            }
        }
    }
}