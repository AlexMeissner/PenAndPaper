using Client.View;
using System.Windows;
using System.Windows.Controls;

namespace Client.Controls
{
    public partial class MapOverviewControl : UserControl
    {
        public MapOverviewControl()
        {
            InitializeComponent();
        }

        private void OnCreateMap(object sender, RoutedEventArgs e)
        {
            MapCreationWindow mapCreationWindow = new();

            if (mapCreationWindow.ShowDialog() == true)
            {

            }
        }
    }
}