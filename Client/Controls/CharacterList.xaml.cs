using Client.Services;
using Client.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using static Client.Services.ServiceExtension;

namespace Client.Controls
{
    [TransistentService]
    public partial class CharacterList : UserControl
    {
        public CharacterListViewModel ViewModel { get; init; }

        public CharacterList(IViewModelProvider viewModelProvider)
        {
            ViewModel = viewModelProvider.Get<CharacterListViewModel>();
            InitializeComponent();

            var collectionView = (CollectionView)CollectionViewSource.GetDefaultView(CharacterListView.ItemsSource);
            collectionView.Filter = ViewModel.OnFilter;
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.Load();
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            ViewModel.UnsubscribeEventHandlers();
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed &&
                sender is ListView listView &&
                listView.SelectedItem is not null)
            {
                DragDrop.DoDragDrop(this, listView.SelectedItem, DragDropEffects.Copy);
            }
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(CharacterListView.ItemsSource).Refresh();
        }
    }
}
