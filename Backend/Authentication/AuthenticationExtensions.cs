using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Authentication;

public static class AuthenticationExtensions
{
    static bool IsApiRequest(HttpRequest request) => request.Path.StartsWithSegments("/api") || request.Path.StartsWithSegments("/account");

    public static IServiceCollection AddGoogleAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        }).AddCookie(options =>
        {
            options.Cookie.Name = "reactapp.auth";
            options.Cookie.HttpOnly = true;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.Cookie.SameSite = SameSiteMode.Lax;
            options.ExpireTimeSpan = TimeSpan.FromHours(8);
            options.SlidingExpiration = true;
            options.LoginPath = "/account/login";
            options.ReturnUrlParameter = "returnUrl";

            // fetch calls must never be redirected to Google; they receive 401 and the SPA handles it.
            // Browser navigations (e.g. deep links in production) are redirected to the login.
            options.Events.OnRedirectToLogin = context =>
            {
                if (IsApiRequest(context.Request))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                }
                else
                {
                    context.Response.Redirect(context.RedirectUri);
                }

                return Task.CompletedTask;
            };

            options.Events.OnRedirectToAccessDenied = context =>
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return Task.CompletedTask;
            };
        }).AddGoogle(options =>
        {
            options.ClientId = configuration["Google:ClientId"]
                ?? throw new InvalidOperationException("Configuration value 'Google:ClientId' is missing.");
            options.ClientSecret = configuration["Google:ClientSecret"]
                ?? throw new InvalidOperationException("Configuration value 'Google:ClientSecret' is missing.");

            options.ClaimActions.MapJsonKey("urn:google:picture", "picture");

            // A failed or cancelled Google login
            options.Events.OnRemoteFailure = async context =>
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "text/html; charset=utf-8";
                await context.Response.WriteAsync(
                    "<!DOCTYPE html><html><body><h1>Login failed</h1>" +
                    "<p>The Google login was cancelled or failed.</p>" +
                    "<p><a href=\"/\">Try again</a></p></body></html>");
                context.HandleResponse();
            };
        });

        // Everything requires an authenticated user unless explicitly marked with [AllowAnonymous].
        services.AddAuthorizationBuilder()
            .SetFallbackPolicy(new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build());

        return services;
    }

    public static WebApplication UseCsrfProtection(this WebApplication app)
    {
        // CSRF protection: API calls and state-changing account calls must carry a custom header.
        // A foreign site cannot send it cross-origin because no CORS permission is granted.
        app.Use(async (context, next) =>
        {
            var request = context.Request;
            var requiresCsrfHeader =
                request.Path.StartsWithSegments("/api") ||
                (request.Path.StartsWithSegments("/account") && !HttpMethods.IsGet(request.Method) && !HttpMethods.IsHead(request.Method));

            if (requiresCsrfHeader && request.Headers["X-CSRF"] != "1")
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return;
            }

            await next();
        });

        return app;
    }
}
