using Microsoft.EntityFrameworkCore.Migrations.Internal;
using UAS.Persistence;

namespace UAS.API.Startup;

public static class ExecuteMigrations
{
    public static IApplicationBuilder ExecuteDbMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Migrator>>();
        var initializer = scope.ServiceProvider.GetRequiredService<DbContextInitialiser>();

        logger.LogInformation("Starting DB migration");

        initializer.Initialize();

        logger.LogInformation("DB migration complete");
        return app;
    }
}