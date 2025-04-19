using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TS.Persistence.Context;

namespace TS.Persistence;

public partial class DbContextInitialiser
{
    public readonly TransactionsDbContext _context;
    public readonly ILogger<DbContextInitialiser> _logger;
    
    public DbContextInitialiser(TransactionsDbContext context, ILogger<DbContextInitialiser> logger)
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