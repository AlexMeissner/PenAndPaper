using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Controls;
using static Client.Services.ServiceExtension;

namespace Client.Services
{
    public interface IControlProvider
    {
        public T Get<T>() where T : UserControl;
    }

    [SingletonService]
    public class ControlProvider : IControlProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public ControlProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public T Get<T>() where T : UserControl
        {
            if (_serviceProvider.GetRequiredService(typeof(T)) is T control)
            {
                return control;
            }

            throw new ArgumentException(string.Format("Unknown view model type {0}", nameof(T)));
        }
    }
}
