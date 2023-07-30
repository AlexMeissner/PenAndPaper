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

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetService<MainWindow>()!;
            mainWindow.Show();
        }
    }
}