using Client.Services;
using Client.Services.API;
using Client.View;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace Client
{
    public partial class App : Application
    {
        private readonly IServiceProvider ServiceProvider;

        public App()
        {
            ServiceCollection services = new();
            ConfigureServices(services);
            ConfigureViews(services);
            ServiceProvider = services.BuildServiceProvider();
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton<IPageNavigator, PageNavigator>();
            services.AddSingleton<ISessionData>(new SessionData());
            services.AddTransient<ILoginApi, LoginApi>();
        }

        private static void ConfigureViews(ServiceCollection services)
        {
            services.AddTransient<MainWindow>();
            services.AddTransient<LoginPage>();
            services.AddTransient<CampaignSelectionPage>();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var mainWindow = ServiceProvider.GetService<MainWindow>()!;
            mainWindow.Show();
        }
    }
}