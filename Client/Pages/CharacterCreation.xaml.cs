using Client.Services;
using Client.ViewModels;
using System.Windows.Controls;
using static Client.Services.ServiceExtension;

namespace Client.Pages
{
    [TransistentService]
    public partial class CharacterCreation : Page
    {
        public CharacterCreationViewModel ViewModel { get; }

        public CharacterCreation(IViewModelProvider viewModelProvider)
        {
            ViewModel = viewModelProvider.Get<CharacterCreationViewModel>();
            InitializeComponent();
        }
    }
}
