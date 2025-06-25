using ApiClient;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;
using Website.Services;

namespace Website.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApis(this IServiceCollection services)
    {
        services.AddSingleton<IEndPointProvider, EndPointProvider>();

        services.AddTransient<IRequestBuilder, RequestBuilder>();
        services.AddScoped<ProtectedSessionStorage>();
        services.AddScoped<ITokenProvider, TokenProvider>();

        services.AddTransient<ICampaignApi, CampaignApi>();
        services.AddTransient<ICharacterApi, CharacterApi>();
        services.AddTransient<IChatApi, ChatApi>();
        services.AddTransient<IInitiativeApi, InitiativeApi>();
        services.AddTransient<IMapApi, MapApi>();
        services.AddTransient<IMonsterApi, MonsterApi>();
        services.AddTransient<IMouseApi, MouseApi>();
        services.AddTransient<IRollApi, RollApi>();
        services.AddTransient<IScriptApi, ScriptApi>();
        services.AddTransient<IAudioApi, AudioApi>();
        services.AddTransient<ITokenApi, TokenApi>();
        services.AddTransient<IUserApi, UserApi>();
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