using Client.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Client.Services
{
    public interface IViewModelProvider
    {
        public T Get<T>() where T : BaseViewModel;
    }

    public class ViewModelProvider : IViewModelProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public ViewModelProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public T Get<T>() where T : BaseViewModel
        {
            if (_serviceProvider.GetRequiredService(typeof(T)) is T viewModel)
            {
                return viewModel;
            }

            throw new ArgumentException(string.Format("Unknown view model type {0}", nameof(T)));
        }
    }
}