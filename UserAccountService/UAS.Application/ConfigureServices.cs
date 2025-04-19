using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UAS.Application.Helper;

namespace UAS.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration configuration)
    {

        services.AddScoped<AccountHelper>();
        ////what this configuration do ,the service is it will search in the app layer for the Irequest and Irequesthandler w bet hattun sawa, so kaeano dictionnary sarit, for each Irequest eenda its own request handler
        var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.StartsWith("UAS.Application"))
            .ToArray();

        // Register MediatR with each assembly
        foreach (var assembly in assemblies)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(assembly);
                cfg.Lifetime = ServiceLifetime.Scoped;
            });
        }

        return services;
    }
}