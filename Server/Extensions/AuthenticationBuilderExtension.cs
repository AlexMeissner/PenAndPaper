using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Server.Extensions;

public static class AuthenticationBuilderExtension
{
    public static AuthenticationBuilder AddGoogle(this AuthenticationBuilder builder)
    {
        builder.AddJwtBearer(options =>
        {
            options.Authority = "https://accounts.google.com";

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = "https://accounts.google.com", // Google's OAuth 2.0 token issuer
                ValidateAudience = true,
                ValidAudience = "493787353591-hjkj98958ovthlv9omga36r32upg5i65.apps.googleusercontent.com",
                ValidateLifetime = true
            };

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();

                    if (!string.IsNullOrEmpty(token))
                    {
                        context.Token = token;
                    }

                    return Task.CompletedTask;
                },
                OnTokenValidated = async context =>
                {
                    if (context.SecurityToken is JwtSecurityToken token)
                    {
                        var httpClient = new HttpClient();
                        var tokenInfoResponse = await httpClient.GetAsync($"https://oauth2.googleapis.com/tokeninfo?access_token={token}");

                        if (!tokenInfoResponse.IsSuccessStatusCode)
                        {
                            context.Fail("Invalid Google OAuth token");
                        }
                    }
                }
            };
        });

        return builder;
    }
}
