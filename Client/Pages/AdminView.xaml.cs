using Client.ViewModels;
using System.Windows.Controls;
using static Client.Services.ServiceExtension;

namespace Client.Pages
{
    [TransistentService]
    public partial class AdminView : Page
    {
        public AdminViewViewModel ViewModel { get; }

        public AdminView(AdminViewViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }
    }
}
