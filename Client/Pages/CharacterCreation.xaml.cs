using Client.ViewModels;
using System.Windows.Controls;
using static Client.Services.ServiceExtension;

namespace Client.Pages
{
    [TransistentService]
    public partial class CharacterCreation : Page
    {
        public CharacterCreationViewModel ViewModel => (CharacterCreationViewModel)DataContext;

        public CharacterCreation()
        {
            DataContext = new CharacterCreationViewModel();
            InitializeComponent();
        }
    }
}
