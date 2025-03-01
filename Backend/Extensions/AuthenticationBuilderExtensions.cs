using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Extensions;

public static class AuthenticationBuilderExtension
{
    public static AuthenticationBuilder AddGoogle(this AuthenticationBuilder builder, IConfiguration configuration)
    {
        builder.AddJwtBearer(options =>
        {
            options.Authority = configuration["Google:Authority"];

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = configuration["Google:ValidIssuer"],
                ValidateAudience = true,
                ValidAudience = configuration["Google:ValidAudience"],
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
                        var endpoint = $"{configuration["Google:ValidAudience"]}?access_token={token}";

                        var httpClient = new HttpClient();
                        var tokenInfoResponse = await httpClient.GetAsync(endpoint);

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