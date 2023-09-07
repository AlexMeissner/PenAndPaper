using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using static Client.Services.ServiceExtension;

namespace Client.Services
{
    public interface IPopupPage
    {
        Page? Content { get; }
        Visibility Visibility { get; }

        public void Open<T>() where T : Page;
        void Close();
    }

    [SingletonService]
    public class PopupPage : IPopupPage, INotifyPropertyChanged
    {
        public Visibility Visibility { get; private set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        private readonly IServiceProvider _serviceProvider;

        public PopupPage(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        private Page? _content;

        public Page? Content
        {
            get => _content;
            set
            {
                _content = value;
                OnPropertyChanged(nameof(Content));
            }
        }

        public void Close()
        {
            Visibility = Visibility.Collapsed;
            _content = null;
        }

        public void Open<T>() where T : Page
        {
            if (Content?.GetType() != typeof(T))
            {
                var oldPage = Content;
                Content = _serviceProvider.GetRequiredService<T>();

                if (oldPage is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
