using System.Security.Claims;
using ApiClient;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.OAuth;
using Website.Services;

namespace Website.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApis(this IServiceCollection services)
    {
        services.AddSingleton<IEndPointProvider, EndPointProvider>();
        services.AddSingleton<ITokenProvider, TokenProvider>();
        
        services.AddTransient<IRequest, Request>();

        services.AddTransient<ICampaignApi, CampaignApi>();
        services.AddTransient<ICharacterApi, CharacterApi>();
        services.AddTransient<IChatApi, ChatApi>();
        services.AddTransient<IMapApi, MapApi>();
        services.AddTransient<IMonsterApi, MonsterApi>();
        services.AddTransient<IMouseApi, MouseApi>();
        services.AddTransient<IRollApi, RollApi>();
        services.AddTransient<IScriptApi, ScriptApi>();
        services.AddTransient<ISoundApi, SoundApi>();
        services.AddTransient<ITokenApi, TokenApi>();
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