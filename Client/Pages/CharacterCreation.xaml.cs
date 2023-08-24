using Client.ViewModels;
using System.Windows.Controls;

namespace Client.Pages
{
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
