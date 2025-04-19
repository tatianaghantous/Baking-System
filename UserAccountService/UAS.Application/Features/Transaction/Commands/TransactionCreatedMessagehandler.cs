using Foxera.RabitMq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UAS.Contracts.Persistence;

namespace UAS.Application.Features.Transaction.Commands;

public class TransactionCreatedMessagehandler
{
    private readonly RabbitMQService _rabbitMqService;
    private readonly string _queueName = "transaction-created"; 
    public readonly IAccountsDbContext _dbContext;
    private readonly IServiceScopeFactory _scopeFactory;
    public class TransactionMessage
    {
        public Domain.Entities.Account Account { get; set; }
        public decimal Amount { get; set; }
    }

    public TransactionCreatedMessagehandler(RabbitMQService rabbitMqService, IAccountsDbContext dbContext, IServiceScopeFactory scopeFactory)
    {
        _rabbitMqService = rabbitMqService;
        _scopeFactory = scopeFactory;

    }

    public async Task StartListening()
    {
        await _rabbitMqService.Receive<TransactionMessage>(_queueName, HandleMessage);
    }

    private async Task HandleMessage(TransactionMessage message)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<IAccountsDbContext>();
            var dbAccount = await dbContext.Account.AsTracking().SingleOrDefaultAsync(a => a.UserId == message.Account.UserId);

            if (dbAccount != null)
            {
                dbAccount.Balance += message.Amount;
                await dbContext.SaveChangesAsync();
            }
        }
    }
}

