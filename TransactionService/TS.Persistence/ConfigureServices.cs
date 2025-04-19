using Foxera.Common.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TS.Contracts.Persistence;
using TS.Persistence.Context;
using TS.Persistence.Settings;

namespace TS.Persistence;

public static class ConfigureServices
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.GetGenericSettings<StorageSettings>(configuration);
        //services.AddHttpContextAccessor();
        services.AddScoped<ITransactionsDbContext, TransactionsDbContext>();//we configure them and add them the services
        services.AddScoped<TransactionsDbContext>();
        services.AddScoped<DbContextInitialiser>();
        services.AddMemoryCache();

        return services;
    }
}