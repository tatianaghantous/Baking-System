using Foxera.Common.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UAS.Contracts.Persistence;
using UAS.Persistence.Context;
using UAS.Persistence.Settings;

namespace UAS.Persistence;

public static class ConfigureServices
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.GetGenericSettings<StorageSettings>(configuration);
        //services.AddHttpContextAccessor();
        services.AddScoped<IAccountsDbContext, AccountsDbContext>();//we configure them and add them the services
        services.AddScoped<AccountsDbContext>();
        services.AddScoped<DbContextInitialiser>();
        services.AddMemoryCache();

        return services;
    }
}