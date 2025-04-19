using Foxera.RabitMq;
using Microsoft.Extensions.DependencyInjection;
using TS.Contracts.Persistence;

namespace TS.Persistence.Features.Account;

public class AccountCreatedMessagehandler
{
    private readonly string _queueName = "account-created"; 
    private readonly IServiceScopeFactory _scopeFactory;
   
    public AccountCreatedMessagehandler(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task StartListening()
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var _rabbitMqService = scope.ServiceProvider.GetRequiredService<RabbitMQService>();
            await _rabbitMqService.Receive<Domain.Entities.Account>(_queueName, HandleMessage);
        }
    }

    private async Task HandleMessage(Domain.Entities.Account account)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ITransactionsDbContext>();
            await dbContext.Account.AddAsync(account);
            await dbContext.SaveChangesAsync();
        }
    }
}

