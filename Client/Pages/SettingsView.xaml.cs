using Client.Services;
using System.Windows.Controls;
using static Client.Services.ServiceExtension;

namespace Client.Pages
{
    [TransistentService]
    public partial class SettingsView : Page
    {
        public ISettings Settings { get; }

        public SettingsView(ISettings settings)
        {
            Settings = settings;

            InitializeComponent();
        }
    }
}
