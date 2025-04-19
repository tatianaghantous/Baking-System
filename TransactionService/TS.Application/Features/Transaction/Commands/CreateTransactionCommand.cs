using AutoMapper;
using Foxera.Common.CustomExceptions;
using Foxera.RabitMq;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Contracts.Persistence;
using TS.Domain.DTOs;

namespace TS.Persistence.Features.Transaction.Commands;
public class TransactionMessage
{
    public Domain.Entities.Account Account { get; set; }
    public decimal Amount { get; set; }
}

public class CreateTransactionCommand : IRequest<TransactionDto>
{
    public int AccountId { get; set; }
    public decimal Amount { get; set; }
    public int TransactionTypeId { get; set; }
    public int CurrencyId { get; set; }
    public int PeriodTypeId  { get; set; }
}
public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, TransactionDto>
{
    private readonly ITransactionsDbContext _transactionsDbContext;
    private readonly IMapper _mapper;
    private readonly IRabbitMQService _rabbitMQService;

    public CreateTransactionCommandHandler(
        ITransactionsDbContext transactionsDbContext,
        IMapper mapper, IRabbitMQService rabbitMqService)
    {
        _transactionsDbContext = transactionsDbContext;
        _mapper = mapper;
        _rabbitMQService = rabbitMqService;
    }

    public async Task<TransactionDto> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        // Ensure the account exists
        var account = await GetByIdAsync(request.AccountId);
        if (account == null || account.IsDeleted == true)
        {
            throw new KeyNotFoundException($"Account with Id {request.AccountId} not found.");
        }
        // Create the transaction entity
        var transaction = new Domain.Entities.Transaction
        {
            AccountId = request.AccountId,
            Amount = request.Amount,
            TransactionTypeId = request.TransactionTypeId,
            CurrencyId = request.CurrencyId,
            CreatedAt = DateTime.Now,
            IsDeleted = false
        };

        // Save the transaction to the database
        await _transactionsDbContext.Transaction.AddAsync(transaction);
        await _transactionsDbContext.SaveChangesAsync(cancellationToken);

        // Check if the transaction is recurrent
        if (transaction.TransactionType.TypeName == "Recurrent")
        {
            // Fetch the period type from the database
            var periodType = await _transactionsDbContext.PeriodType
                .FindAsync(request.PeriodTypeId);
            if (periodType == null)
            {
                throw new InvalidOperationException($"PeriodType with Id {request.PeriodTypeId} not found.");
            }
        
            // Create and save the recurrent transaction
            var recurrentTransaction = new Domain.Entities.RecurrentTransaction
            {
                TransactionId = transaction.Id,
                NextExecutionDate = CalculateNextExecutionDate(periodType.Name),
                PeriodTypeId = periodType.Id,
                PeriodType = periodType
            };
        
            await _transactionsDbContext.RecurrentTransaction.AddAsync(recurrentTransaction);
            await _transactionsDbContext.SaveChangesAsync(cancellationToken);
            var message = new TransactionMessage
            {
                Account = account,
                Amount = transaction.Amount
            };
            _rabbitMQService.Send("transaction-created",message);
        }

        // Return the DTO
        return _mapper.Map<TransactionDto>(transaction);
    }
    
    public async Task<Domain.Entities.Account> GetByIdAsync(int id)
    {
        return await _transactionsDbContext.Account
            .FirstOrDefaultAsync(a => a.Id == id && (a.IsDeleted == null || a.IsDeleted == false)) ?? throw new NotFoundException("Account Not Found");
    }
    private DateTime CalculateNextExecutionDate(string periodTypeName)
    {
        var now = DateTime.Now;
        switch (periodTypeName)
        {
            case "Daily":
                return now.AddDays(1); // Next day
            case "Weekly":
                return now.AddDays(7); // Next week
            case "Monthly":
                return now.AddMonths(1); // Next month
            case "Yearly":
                return now.AddYears(1); // Next year
            default:
                throw new InvalidOperationException("Unsupported period type.");
        }
    }
}