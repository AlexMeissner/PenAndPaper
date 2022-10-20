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
            services.AddTransient<IAuthenticationApi, AuthenticationApi>();
            services.AddTransient<ICampaignCreationApi, CampaignCreationApi>();
            services.AddTransient<IUserApi, UserApi>();
        }

        private static void ConfigureViews(ServiceCollection services)
        {
            services.AddTransient<MainWindow>();
            services.AddTransient<LoginPage>();
            services.AddTransient<RegisterPage>();
            services.AddTransient<CampaignSelectionPage>();
            services.AddTransient<CampaignCreationPage>();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var mainWindow = ServiceProvider.GetService<MainWindow>()!;
            mainWindow.Show();
        }
    }
}