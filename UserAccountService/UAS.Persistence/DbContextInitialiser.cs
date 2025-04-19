using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UAS.Domain.Entities;
using UAS.Persistence.Context;

namespace UAS.Persistence;

public partial class DbContextInitialiser
{
    public readonly AccountsDbContext _context;
    public readonly ILogger<DbContextInitialiser> _logger;
    
    public DbContextInitialiser(AccountsDbContext context, ILogger<DbContextInitialiser> logger)
    {
        _context = context;
        _logger = logger;
    }

    public void Initialize()
    {
        try
        {
            if (_context.Database.IsNpgsql())
            {
                var pendingMigrations = _context.Database.GetPendingMigrations();

                if (pendingMigrations.Any())
                {
                    _context.Database.Migrate();
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initializing the database.");
            throw;
        }
    }

}