using System.Threading.Channels;
using Backend.Services;
using Backend.Services.Repositories;
using DataTransfer.Chat;
using DataTransfer.Dice;
using DataTransfer.Grid;
using DataTransfer.Map;
using DataTransfer.Mouse;
using DataTransfer.Sound;
using DataTransfer.Token;

namespace Backend.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddChannels(this IServiceCollection services)
    {
        var options = new UnboundedChannelOptions()
        {
            SingleReader = true,
            AllowSynchronousContinuations = false,
        };

        services.AddSingleton<Channel<ChatMessageEventArgs>>(
            _ => Channel.CreateUnbounded<ChatMessageEventArgs>(options));
        services.AddSingleton<Channel<DiceRolledEventArgs>>(
            _ => Channel.CreateUnbounded<DiceRolledEventArgs>(options));
        services.AddSingleton<Channel<GridChangedEventArgs>>(
            _ => Channel.CreateUnbounded<GridChangedEventArgs>(options));
        services.AddSingleton<Channel<MapChangedEventArgs>>(
            _ => Channel.CreateUnbounded<MapChangedEventArgs>(options));
        services.AddSingleton<Channel<MapCollectionChangedEventArgs>>(
            _ => Channel.CreateUnbounded<MapCollectionChangedEventArgs>(options));
        services.AddSingleton<Channel<MouseMoveEventArgs>>(
            _ => Channel.CreateUnbounded<MouseMoveEventArgs>(options));
        services.AddSingleton<Channel<SoundStartedEventArgs>>(
            _ => Channel.CreateUnbounded<SoundStartedEventArgs>(options));
        services.AddSingleton<Channel<SoundStoppedEventArgs>>(
            _ => Channel.CreateUnbounded<SoundStoppedEventArgs>(options));
        services.AddSingleton<Channel<TokenAddedEventArgs>>(
            _ => Channel.CreateUnbounded<TokenAddedEventArgs>(options));
        services.AddSingleton<Channel<TokenMovedEventArgs>>(
            _ => Channel.CreateUnbounded<TokenMovedEventArgs>(options));

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