using Client.Services;
using Client.Windows;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

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
            mainWindow.Show();

            //var testWindow = new TestWindow();
            //testWindow.Show();

            var updateNotifier = (UpdateNotifier)_serviceProvider.GetRequiredService<IUpdateNotifier>();
            await updateNotifier.Connect();
        }

        private async void OnShutdown(object sender, ExitEventArgs e)
        {
            var updateNotifier = (UpdateNotifier)_serviceProvider.GetRequiredService<IUpdateNotifier>();
            await updateNotifier.Close();
        }
    }
}