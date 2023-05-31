using Client.Controls;
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
            services.AddTransient<ICache, Cache>();
            services.AddSingleton<IPageNavigator, PageNavigator>();
            services.AddSingleton<ISessionData>(new SessionData());
            services.AddTransient<IAudioPlayer, AudioPlayer>();
            services.AddTransient<IEndPointProvider, EndPointProvider>();
            services.AddTransient<IAuthenticationApi, AuthenticationApi>();
            services.AddTransient<IActiveMapApi, ActiveMapApi>();
            services.AddTransient<ICampaignCreationApi, CampaignCreationApi>();
            services.AddTransient<ICampaignOverviewApi, CampaignOverviewApi>();
            services.AddTransient<ICampaignUpdatesApi, CampaignUpdatesApi>();
            services.AddTransient<ICampaignUpdates, CampaignUpdates>();
            services.AddTransient<IMapApi, MapApi>();
            services.AddTransient<IMapOverviewApi, MapOverviewApi>();
            services.AddTransient<IUserApi, UserApi>();
            services.AddTransient<IRollApi, RollApi>();
            services.AddTransient<ISoundApi, SoundApi>();
        }

        private static void ConfigureViews(ServiceCollection services)
        {
            services.AddTransient<MainWindow>();
            services.AddTransient<LoginPage>();
            services.AddTransient<RegisterPage>();
            services.AddTransient<CampaignSelectionPage>();
            services.AddTransient<CampaignCreationPage>();
            services.AddTransient<CampaignGamemasterViewPage>();
            services.AddTransient<CampaignPlayerViewPage>();
            services.AddTransient<MapControl>();
            services.AddTransient<MapOverviewControl>();
            services.AddTransient<GamemasterMusicControl>();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var mainWindow = ServiceProvider.GetService<MainWindow>()!;
            mainWindow.Show();
        }
    }
}