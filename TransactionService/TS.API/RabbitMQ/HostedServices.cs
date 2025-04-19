using TS.Persistence.Features.Account;

namespace TS.API.RabbitMQ;

public class HostedServices: BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public HostedServices(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var accountCreation = scope.ServiceProvider.GetRequiredService<AccountCreatedMessagehandler>();
            accountCreation.StartListening();
        }

        return Task.CompletedTask;
    }
}
