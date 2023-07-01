using Client.Services;
using System.Windows;
using System.Windows.Navigation;

namespace Client.View
{
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

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            int margin = (WindowState == WindowState.Maximized) ? 5 : 0;
            MainGrid.Margin = new(margin);
        }
    }
}