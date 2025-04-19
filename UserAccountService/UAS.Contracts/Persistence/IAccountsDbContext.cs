using Microsoft.EntityFrameworkCore;
using UAS.Domain.Entities;

namespace UAS.Contracts.Persistence;

public interface IAccountsDbContext
{
    
    public  DbSet<Account> Account { get; set; }

    public  DbSet<Branch> Branch { get; set; }

    public  DbSet<Location> Location { get; set; }

    public  DbSet<Role> Role { get; set; }

    public  DbSet<User> User { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}