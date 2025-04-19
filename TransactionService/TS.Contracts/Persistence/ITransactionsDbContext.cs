using Microsoft.EntityFrameworkCore;
using TS.Domain.Entities;

namespace TS.Contracts.Persistence;

public interface ITransactionsDbContext
{
    public  DbSet<Account> Account { get; set; }

    public  DbSet<Currency> Currency { get; set; }

    public  DbSet<RecurrentTransaction> RecurrentTransaction { get; set; }

    public  DbSet<Transaction> Transaction { get; set; }
    public  DbSet<PeriodType> PeriodType { get; set; }
    public  DbSet<Transactiontype> Transactiontype { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}