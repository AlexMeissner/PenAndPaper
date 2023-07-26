using Client.Services;
using Client.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static Client.Services.ServiceExtension;

namespace Client.Controls
{
    [TransistentService]
    public partial class CharacterList : UserControl
    {
        public CharacterListViewModel ViewModel => (CharacterListViewModel)DataContext;

        public CharacterList(IViewModelProvider viewModelProvider)
        {
            InitializeComponent();
            DataContext = viewModelProvider.Get<CharacterListViewModel>();
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            // Bind this function instead
            await ViewModel.Load();
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && sender is ListView listView)
            {
                DragDrop.DoDragDrop(this, listView.SelectedItem, DragDropEffects.Copy);
            }
        }
    }
}
