using Client.Services;
using System.Windows.Controls;

namespace Client.Controls
{
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
