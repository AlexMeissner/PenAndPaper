using Client.Services;
using Client.ViewModels;
using System.Windows;
using static Client.Services.ServiceExtension;

namespace Client.Windows
{
    [TransistentService]
    public partial class DebugWindow : Window
    {
        public DebugViewModel ViewModel { get; init; }

        public DebugWindow(IViewModelProvider viewModelProvider)
        {
            ViewModel = viewModelProvider.Get<DebugViewModel>();

            InitializeComponent();
        }
    }
}
