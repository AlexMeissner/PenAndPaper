using System.Threading.Channels;
using Backend.Chat;
using Backend.Services;
using Backend.Services.Repositories;
using DataTransfer.Dice;
using DataTransfer.Grid;
using DataTransfer.Map;
using DataTransfer.Mouse;
using DataTransfer.Sound;
using DataTransfer.Token;

namespace Backend.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBackgroundServices(this IServiceCollection services)
    {
        services.AddHostedService<ChatMessageRelayService>();

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
        services.AddSingleton(_ => Channel.CreateUnbounded<DiceRolledEventArgs>(options));
        services.AddSingleton(_ => Channel.CreateUnbounded<GridChangedEventArgs>(options));
        services.AddSingleton(_ => Channel.CreateUnbounded<MapChangedEventArgs>(options));
        services.AddSingleton(_ => Channel.CreateUnbounded<MapCollectionChangedEventArgs>(options));
        services.AddSingleton(_ => Channel.CreateUnbounded<MouseMoveEventArgs>(options));
        services.AddSingleton(_ => Channel.CreateUnbounded<SoundStartedEventArgs>(options));
        services.AddSingleton(_ => Channel.CreateUnbounded<SoundStoppedEventArgs>(options));
        services.AddSingleton(_ => Channel.CreateUnbounded<TokenAddedEventArgs>(options));
        services.AddSingleton(_ => Channel.CreateUnbounded<TokenMovedEventArgs>(options));

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<ICampaignRepository, CampaignRepository>();
        services.AddTransient<ICharacterRepository, CharacterRepository>();
        services.AddTransient<IMapRepository, MapRepository>();
        services.AddTransient<IMonsterRepository, MonsterRepository>();
        services.AddTransient<IScriptRepository, ScriptRepository>();
        services.AddTransient<ITokenRepository, TokenRepository>();
        services.AddTransient<IUserRepository, UserRepository>();

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