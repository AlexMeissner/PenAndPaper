using Backend.Services;
using Backend.Services.Repositories;

namespace Backend.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<ICampaignRepository, CampaignRepository>();
        services.AddTransient<ICharacterRepository, CharacterRepository>();
        services.AddTransient<IMapRepository, MapRepository>();
        services.AddTransient<IMonsterRepository, MonsterRepository>();
        services.AddTransient<IScriptRepository, ScriptRepository>();
        services.AddTransient<ITokenRepository, TokenRepository>();

        return services;
    }

    public static IServiceCollection AddScopedServices(this IServiceCollection services)
    {
        services.AddScoped<IIdentity, Identity>();
        
        return services;
    }
}