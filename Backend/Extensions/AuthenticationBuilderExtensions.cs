using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Backend.Extensions;

public static class AuthenticationBuilderExtension
{
    public static AuthenticationBuilder AddCustomJwt(this AuthenticationBuilder builder, IConfiguration configuration)
    {
        builder.AddJwtBearer("Bearer", options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = configuration["JWT:Issuer"],
                ValidAudience = configuration["JWT:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"] ?? throw new NullReferenceException("JWT key not found")))
            };
        });

        return builder;
    }
}