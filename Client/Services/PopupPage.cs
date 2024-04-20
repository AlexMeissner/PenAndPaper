using Client.Commands;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static Client.Services.ServiceExtension;

namespace Client.Services
{
    public interface IPopupPage : INotifyPropertyChanged
    {
        public string Title { get; }
        public Page? Content { get; }
        public Visibility Visibility { get; }
        public ICommand CloseCommand { get; }
        public void Close();
        public T Open<T>(string title) where T : Page;
    }

    [TransistentService]
    public class PopupPage : IPopupPage
    {
        private readonly IServiceProvider _serviceProvider;

        private string _title = string.Empty;
        private Page? _content;
        private Visibility _visibility = Visibility.Collapsed;

        public string Title
        {
            get => _title;

            private set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        public Page? Content
        {
            get => _content;

            private set
            {
                _content = value;
                OnPropertyChanged(nameof(Content));
            }
        }

        public Visibility Visibility
        {
            get => _visibility;
            private set
            {
                _visibility = value;
                OnPropertyChanged(nameof(Visibility));
            }
        }

        public ICommand CloseCommand { get; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public PopupPage(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            CloseCommand = new RelayCommand(Close);
        }

        public T Open<T>(string title) where T : Page
        {
            Close();

            if (Content?.GetType() != typeof(T))
            {
                var oldPage = Content;

                Title = title;
                Content = _serviceProvider.GetRequiredService<T>();
                Visibility = Visibility.Visible;

                if (oldPage is IDisposable disposable)
                {
                    disposable.Dispose();
                }

                return (T)Content;
            }

            throw new ArgumentException("No Page of type {name} found", nameof(T));
        }

        public void Close()
        {
            Visibility = Visibility.Collapsed;
            Content = null;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
