using Client.Services;
using Client.Windows;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using System.Windows.Input;

namespace Client
{
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;

        public App()
        {
            var services = new ServiceCollection();
            services.RegisterServices();
            _serviceProvider = services.BuildServiceProvider();
        }

        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.PreviewKeyDown += OnKeyDown;
            mainWindow.Show();

            var updateNotifier = (UpdateNotifier)_serviceProvider.GetRequiredService<IUpdateNotifier>();
            await updateNotifier.Connect();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            // Ctrl + Shift + 'D'
            if (Keyboard.Modifiers == (ModifierKeys.Control | ModifierKeys.Shift) && e.Key == Key.D)
            {
                var debugWindow = _serviceProvider.GetRequiredService<DebugWindow>();
                debugWindow.Show();
            }
        }

        private async void OnShutdown(object sender, ExitEventArgs e)
        {
            var updateNotifier = (UpdateNotifier)_serviceProvider.GetRequiredService<IUpdateNotifier>();
            await updateNotifier.Close();
        }
    }
}