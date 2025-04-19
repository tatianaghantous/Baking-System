using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;
using TS.Persistence.Features.Account;

namespace TS.API;

public static class ConfigureServices
{
    public static void AddWebApiServices(this IServiceCollection services, IConfiguration configuration)
    {

        string defaultVersionString = configuration.GetSection("ApiVersioning")["DefaultVersion"];
        var versionParts = defaultVersionString.Split('.');
        int majorVersion = int.Parse(versionParts[0]);
        int minorVersion = versionParts.Length > 1 ? int.Parse(versionParts[1]) : 0;


        services.AddApiVersioning(config =>
        {
            config.DefaultApiVersion = new ApiVersion(majorVersion, minorVersion);
            config.AssumeDefaultVersionWhenUnspecified = true;
            config.ReportApiVersions = true;
        });

        services.AddSingleton<AccountCreatedMessagehandler>();

        var modelBuilder = new ODataModelBuilder();
        services.AddControllers().AddOData(
            options => options
                .Select()    // Enables clients to select specific properties to return
                .Filter()    // Allows clients to filter the results based on conditions
                .OrderBy()   // Permits clients to sort the results based on properties
                .Expand()    // Enables inclusion of related entities in the results
                .Count()     // Allows clients to request a count of the query results
                .SetMaxTop(null) // Removes any maximum limit on returned record count
                .SkipToken() // Added SkipToken for server-driven paging
                .AddRouteComponents("odata", modelBuilder.GetEdmModel()) // Sets up the OData endpoint routing
        );



        services.AddEndpointsApiExplorer();
        //services.AddDatabaseDeveloperPageExceptionFilter();


        services.AddAutoMapper(Assembly.GetAssembly(typeof(Program)));

    }
}