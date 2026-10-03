using Backend.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Net;
using System.Text;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Information)
    .WriteTo.Console()
    .Enrich.FromLogContext()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .CreateLogger();
    builder.Host.UseSerilog(Log.Logger);

    builder.Services.AddControllers();

    builder.Services.Configure<ReturnUrlOptions>(
        builder.Configuration.GetSection(ReturnUrlOptions.SectionName));

    builder.Services
        .AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        })
        .AddCookie(options =>
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
        })
        .AddGoogle(options =>
        {
            options.ClientId = builder.Configuration["Google:ClientId"]
                ?? throw new InvalidOperationException("Configuration value 'Google:ClientId' is missing.");
            options.ClientSecret = builder.Configuration["Google:ClientSecret"]
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
    builder.Services.AddAuthorizationBuilder()
        .SetFallbackPolicy(new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build());

    builder.Services.Configure<ForwardedHeadersOptions>(options =>
    {
        options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;

        // Only trust forwarded headers from the configured reverse proxy (e.g. the Docker network of Caddy).
        foreach (var network in builder.Configuration.GetSection("ReverseProxy:KnownNetworks").Get<string[]>() ?? [])
        {
            options.KnownIPNetworks.Add(System.Net.IPNetwork.Parse(network));
        }

        foreach (var proxy in builder.Configuration.GetSection("ReverseProxy:KnownProxies").Get<string[]>() ?? [])
        {
            options.KnownProxies.Add(IPAddress.Parse(proxy));
        }
    });

    var app = builder.Build();

    app.UseForwardedHeaders();

    app.UseSerilogRequestLogging();

    // In production Caddy terminates TLS and redirects HTTP to HTTPS.
    if (app.Environment.IsDevelopment())
    {
        app.UseHttpsRedirection();
    }

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

    app.UseDefaultFiles();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    if (app.Environment.IsProduction())
    {
        app.MapStaticAssets();
        app.MapFallbackToFile("/index.html");
    }

    app.Run();
}
catch (Exception exception)
{
    Log.Fatal(exception, "An error occurred while starting the application.");
}
finally
{
    Log.CloseAndFlush();
}

static bool IsApiRequest(HttpRequest request) =>
    request.Path.StartsWithSegments("/api") || request.Path.StartsWithSegments("/account");
