using Client.Helper;
using Client.Services;
using Client.Services.API;
using Client.Windows;
using DataTransfer.Login;
using System.Windows;
using System.Windows.Controls;
using static Client.Services.ServiceExtension;

namespace Client.Pages
{
    [TransistentService]
    public partial class RegisterPage : Page
    {
        private readonly IPageNavigator _pageNavigator;
        private readonly IUserApi _userApi;

        public RegisterPage(IPageNavigator pageNavigator, IUserApi userApi)
        {
            InitializeComponent();
            _pageNavigator = pageNavigator;
            _userApi = userApi;
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
                var response = await _userApi.RegisterAsync(userCredentials);

                if (response.Succeded)
                {
                    _pageNavigator.OpenPage<LoginPage>();
                }
                else
                {
                    MessageBoxUtility.Show(response.StatusCode);
                }
            }
        }
    }
}
