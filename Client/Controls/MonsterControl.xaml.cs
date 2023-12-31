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
    public partial class MonsterControl : UserControl
    {
        public MonsterListViewModel ViewModel { get; init; }

        public MonsterControl(IViewModelProvider viewModelProvider)
        {
            ViewModel = viewModelProvider.Get<MonsterListViewModel>();

            InitializeComponent();

            var collectionView = (CollectionView)CollectionViewSource.GetDefaultView(MonstersItemsControl.ItemsSource);
            collectionView.Filter = ViewModel.OnFilter;
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.OnLoaded();
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
            CollectionViewSource.GetDefaultView(MonstersItemsControl.ItemsSource).Refresh();
        }
    }
}
