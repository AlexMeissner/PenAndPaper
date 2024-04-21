using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel;
using System.Windows.Controls;
using static Client.Services.ServiceExtension;

namespace Client.Services
{
    public interface IPageNavigator : INotifyPropertyChanged
    {
        public Page? CurrentPage { get; }
        public void OpenPage<T>() where T : Page;
    }

    [SingletonService]
    public class PageNavigator(IServiceProvider serviceProvider) : IPageNavigator
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private Page? _currentPage;

        public Page? CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged(nameof(CurrentPage));
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void OpenPage<T>() where T : Page
        {
            if (CurrentPage?.GetType() != typeof(T))
            {
                var oldPage = CurrentPage;
                CurrentPage = serviceProvider.GetRequiredService<T>();

                if (oldPage is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
        }
    }
}