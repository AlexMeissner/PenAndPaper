using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Components.Server.Circuits;
using System.Security.Claims;
using Website.Database.Rules;
using Website.Events;
using Website.Services;
using Website.Services.Repositories;

namespace Website.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApis(this IServiceCollection services)
    {
        services.AddScoped<IAudioRepository, AudioRepository>();
        services.AddScoped<ICampaignRepository, CampaignRepository>();
        services.AddScoped<ICharacterRepository, CharacterRepository>();
        services.AddScoped<IChatRepository, ChatRepository>();
        services.AddScoped<IDiceRepository, DiceRepository>();
        services.AddScoped<IInitiativeRepository, InitiativeRepository>();
        services.AddScoped<IMapRepository, MapRepository>();
        services.AddScoped<IMonsterRepository, MonsterRepository>();
        services.AddScoped<ITokenRepository, TokenRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IMonsterParser, MonsterParser>();
        services.AddScoped<IUserClaims, UserClaims>();
        services.AddScoped<CircuitHandler, UserClaimsCircuitHandler>();

        services.AddSingleton<ICampaignEventHub, CampaignEventHub>();
    }

    public static void AddGoogleAuthentication(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddGoogle(configureOptions =>
            {
                configureOptions.ClientId =
                    configuration["Google:ClientId"] ?? throw new Exception("ClientId not found");
                configureOptions.ClientSecret = configuration["Google:ClientSecret"] ??
                                                throw new Exception("ClientSecret not found");
                configureOptions.SaveTokens = true;
                configureOptions.AccessType = "offline";
                configureOptions.Scope.Add("openid");
                configureOptions.Scope.Add("profile");
                configureOptions.Scope.Add("email");
                configureOptions.Events = new OAuthEvents
                {
                    OnCreatingTicket = context =>
                    {
                        if (context.TokenResponse.Response is { } response)
                        {
                            var idToken = response.RootElement.GetProperty("id_token").ToString();
                            context.Identity?.AddClaim(new Claim("id_token", idToken));
                        }

                        return Task.CompletedTask;
                    }
                };
            });
    }
}