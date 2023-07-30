using Client.Commands;
using System.Windows;
using System.Windows.Input;
using static Client.Services.ServiceExtension;

namespace Client.ViewModels
{
    [TransistentService]
    public class ScriptViewModel : BaseViewModel
    {
        public string Markdown { get; set; } = string.Empty;
        public string UneditedText { get; set; } = string.Empty;

        public Visibility EditVisibility { get; set; } = Visibility.Collapsed;
        public Visibility RenderVisibility { get; set; } = Visibility.Visible;

        public ICommand CancelCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand SaveCommand { get; set; }

        public ScriptViewModel()
        {
            CancelCommand = new RelayCommand(OnCancel);
            EditCommand = new RelayCommand(OnEdit);
            SaveCommand = new RelayCommand(OnSave);
        }

        public void OnEdit()
        {
            UneditedText = Markdown;
            EditVisibility = Visibility.Visible;
            RenderVisibility = Visibility.Collapsed;
        }

        public void OnCancel()
        {
            Markdown = UneditedText;
            EditVisibility = Visibility.Collapsed;
            RenderVisibility = Visibility.Visible;
        }

        public void OnSave()
        {
            EditVisibility = Visibility.Collapsed;
            RenderVisibility = Visibility.Visible;
        }
    }
}
