using Backend.Chat;
using Backend.DungeonsAndDragons;
using Backend.MouseIndicators;
using Backend.Rolls;
using Backend.Services;
using Backend.Services.Repositories;
using Backend.Tokens;
using DataTransfer.Grid;
using DataTransfer.Sound;
using DataTransfer.Token;
using System.Threading.Channels;

namespace Backend.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBackgroundServices(this IServiceCollection services)
    {
        services.AddHostedService<ChatMessageRelayService>();
        services.AddHostedService<RollMessageRelayService>();
        services.AddHostedService<MouseIndicatorRelayService>();

        return services;
    }

    public static IServiceCollection AddChannels(this IServiceCollection services)
    {
        var options = new UnboundedChannelOptions()
        {
            SingleReader = true,
            AllowSynchronousContinuations = false,
        };

        services.AddSingleton(_ => Channel.CreateUnbounded<ChatChannelMessage>(options));
        services.AddSingleton(_ => Channel.CreateUnbounded<RollChannelMessage>(options));
        services.AddSingleton(_ => Channel.CreateUnbounded<GridChangedEventArgs>(options));
        services.AddSingleton(_ => Channel.CreateUnbounded<MouseIndicatorChannelMessage>(options));
        services.AddSingleton(_ => Channel.CreateUnbounded<SoundStartedEventArgs>(options));
        services.AddSingleton(_ => Channel.CreateUnbounded<SoundStoppedEventArgs>(options));
        services.AddSingleton(_ => Channel.CreateUnbounded<TokenAddedEventArgs>(options));
        services.AddSingleton(_ => Channel.CreateUnbounded<TokenMovedEventArgs>(options));

        return services;
    }

    public static IServiceCollection AddTransient(this IServiceCollection services)
    {
        services.AddTransient<ICampaignRepository, CampaignRepository>();
        services.AddTransient<ICharacterRepository, CharacterRepository>();
        services.AddTransient<IMapRepository, MapRepository>();
        services.AddTransient<IMonsterRepository, MonsterRepository>();
        services.AddTransient<IScriptRepository, ScriptRepository>();
        services.AddTransient<ITokenRepository, TokenRepository>();
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IDiceRoller, DiceRoller>();

        services.AddTransient<IMonsterParser, MonsterParser>();

        return services;
    }

    public static IServiceCollection AddScopedServices(this IServiceCollection services)
    {
        services.AddScoped<IIdentity, Identity>();

        return services;
    }

    public static IServiceCollection AddSingletonServices(this IServiceCollection services)
    {
        services.AddSingleton<IUserConnectionTracker, UserConnectionTracker>();

        return services;
    }
}