using Foxera.Common.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NHibernate.Classic;
using UAS.Keycloak.Contracts;
using UAS.Keycloak.Settings;

namespace UAS.Keycloak;

public static class ConfigureServices
{
    public static IServiceCollection AddAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = services.GetGenericSettings<IdentitySettings>(configuration); 

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.MetadataAddress = settings.MetadataAddress;
                o.Authority = settings.Authority;
                o.Audience = settings.Audience;
                o.RequireHttpsMetadata = false;
                o.IncludeErrorDetails = true;
                IdentityModelEventSource.ShowPII = true;
                 o.TokenValidationParameters = new TokenValidationParameters()
                 {
                     LifetimeValidator = (_, expires, _, _) => expires > DateTime.UtcNow,
                     ValidateActor = false,
                     ValidateIssuer = true,
                     ValidateAudience = true,
                     RequireSignedTokens = true,
                     ValidateIssuerSigningKey = true,
                     RequireExpirationTime = true,
                     ValidateLifetime = true,
                     ClockSkew = TimeSpan.Zero,
                     RequireAudience = true
                 };

                 o.Events = new JwtBearerEvents()
                 {
                     OnAuthenticationFailed = c =>
                     {
                         c.NoResult();
                         c.Response.StatusCode = 500;
                         c.Response.ContentType = "text/plain";
                         return c.Response.WriteAsync(c.Exception.ToString());
                     },
                     OnChallenge = context =>
                     {
                         context.HandleResponse();
                         context.Response.StatusCode = 401;
                         context.Response.ContentType = "application/json";
                         var result = JsonConvert.SerializeObject("401 Not authorized");
                         return context.Response.WriteAsync(result);
                     },
                     OnForbidden = context =>
                     {
                         context.Response.StatusCode = 403;
                         context.Response.ContentType = "application/json";
                         var result = JsonConvert.SerializeObject("403 Not authorized");
                         return context.Response.WriteAsync(result);
                     },
                };
            });
        
        services.AddHttpContextAccessor();
        services.AddTransient<ICurrentUser, CurrentUser>();
        services.AddTransient<ICurrentContextAccessor, CurrentContextAccessor>();

        return services;
    }
    
}