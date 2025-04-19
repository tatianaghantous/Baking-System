using Microsoft.EntityFrameworkCore;
using TS.Contracts.Persistence;
using TS.Domain.Entities;

namespace TS.Persistence.BackgroundJobs;

public class RecurrentTransactionProcessor
{
    private readonly ITransactionsDbContext _context;

    public RecurrentTransactionProcessor(ITransactionsDbContext context)
    {
        _context = context;
    }

    public async Task ProcessRecurrentTransaction()
    {
        List<RecurrentTransaction> rectransactions = await _context.RecurrentTransaction
            .Include(rt => rt.PeriodType)
            .Include(rt => rt.Transaction)
            .ThenInclude(rt => rt.Account)
            .ToListAsync();

        if (rectransactions == null) throw new InvalidOperationException("Recurrent transaction not found");


            foreach (RecurrentTransaction rectransaction in rectransactions)
            {
                if (rectransaction.NextExecutionDate.Value.Date == DateTime.Today)
                {
                var newTransaction = new Transaction
                {
                    AccountId = rectransaction.Transaction.AccountId,
                    Amount = rectransaction.Transaction.Amount,
                    CreatedAt = DateTime.Now,
                    TransactionTypeId = rectransaction.Transaction.TransactionTypeId,
                    CurrencyId = rectransaction.Transaction.CurrencyId,
                    IsDeleted = false
                };

                _context.Transaction.Add(newTransaction);

                // Update the account balance
                // rectransaction.Transaction.Account.Balance += transaction.Amount;

                // Calculate the next execution date
                rectransaction.NextExecutionDate = CalculateNextExecutionDate(rectransaction);
            }
        }
            await _context.SaveChangesAsync();
    }

    private DateTime CalculateNextExecutionDate(RecurrentTransaction transaction)
    {
        if (transaction.NextExecutionDate == null)
        {
            throw new InvalidOperationException("NextExecutionDate is not set.");
        }

        switch (transaction.PeriodType.Name)
        {
            case "Daily":
                return transaction.NextExecutionDate.Value.AddDays(1);
            case "Weekly":
                return transaction.NextExecutionDate.Value.AddDays(7); 
            case "Monthly":
                return transaction.NextExecutionDate.Value.AddMonths(1);
            case "Yearly":
                return transaction.NextExecutionDate.Value.AddYears(1);
            default:
                throw new InvalidOperationException("Unsupported period type");
        }
    }

}