using UAS.Application.Features.Transaction.Commands;

namespace UAS.API.RabbitMQ;

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
            var accountCreation = scope.ServiceProvider.GetRequiredService<TransactionCreatedMessagehandler>();
            accountCreation.StartListening();
        }

        return Task.CompletedTask;
    }
}
