using System.Windows;
using Client.Helper;

namespace Client.View
{
    public partial class SplashScreen : Window
    {
        public SplashScreen()
        {
            InitializeComponent();            
            SplashImage.Source = RandomBackgroundImage.GetImageFromResource();
        }
    }
}