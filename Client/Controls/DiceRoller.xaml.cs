using Client.Services;
using Client.ViewModels;
using System.Windows.Controls;
using static Client.Services.ServiceExtension;

namespace Client.Controls
{
    [TransistentService]
    public partial class DiceRoller : UserControl
    {
        public DiceRollerViewModel ViewModel => (DiceRollerViewModel)DataContext;

        public DiceRoller(IViewModelProvider viewModelProvider)
        {
            DataContext = viewModelProvider.Get<DiceRollerViewModel>();

            InitializeComponent();
        }
    }
}
