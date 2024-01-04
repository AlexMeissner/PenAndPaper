using System.Windows;
using static Client.Services.ServiceExtension;

namespace Client.Windows
{
    [TransistentService]
    public partial class DebugWindow : Window
    {
        public DebugWindow()
        {
            InitializeComponent();
        }
    }
}
