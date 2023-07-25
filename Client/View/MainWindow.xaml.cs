using Client.Services;
using System.Windows;
using System.Windows.Navigation;
using static Client.Services.ServiceExtension;

namespace Client.View
{
    [TransistentService]
    public partial class MainWindow : CustomWindow
    {
        public IPageNavigator PageNavigator { get; }

        public MainWindow(IPageNavigator pageNavigator)
        {
            PageNavigator = pageNavigator;
            PageNavigator.OpenPage<LoginPage>();

            InitializeComponent();
        }

        private void OnNavigating(object sender, NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Forward || e.NavigationMode == NavigationMode.Back)
            {
                e.Cancel = true;
            }
        }
    }
}