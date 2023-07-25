using Client.Services;
using System.Windows.Controls;
using static Client.Services.ServiceExtension;

namespace Client.Controls
{
    [TransistentService]
    public partial class SettingsControl : UserControl
    {
        public ISettings Settings { get; }

        public SettingsControl(ISettings settings)
        {
            Settings = settings;

            InitializeComponent();
        }
    }
}
