using Client.Services;
using Client.ViewModels;
using System.Windows.Controls;
using static Client.Services.ServiceExtension;

namespace Client.Controls
{
    [TransistentService]
    public partial class Script : UserControl
    {
        public ScriptViewModel ViewModel => (ScriptViewModel)DataContext;

        public Script(IViewModelProvider viewModelProvider)
        {
            DataContext = viewModelProvider.Get<ScriptViewModel>();
            InitializeComponent();
        }
    }
}