using Foxera.Common.CustomExceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UAS.Contracts.Persistence;

namespace UAS.Application.Features.Account.Commands;

public class DeleteAccountCommand: IRequest<bool>
{
    public int AccountId { get; set; }
    
}

public class DeleteAccountCommandHandler : IRequestHandler<DeleteAccountCommand, bool>
{
    private readonly IAccountsDbContext _accountsDbContext;

    public DeleteAccountCommandHandler(IAccountsDbContext accountsDbContext)
    {
        _accountsDbContext = accountsDbContext;
    }

    public async Task<bool> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        // Get the account by ID
        var account = await GetByIdAsync(request.AccountId);

        if (account == null)
        {
            // Account doesn't exist
            return false;
        }

        // Mark the account as deleted
        account.IsDeleted = true;

        // Update the account in the repository
        await UpdateAsync(account);

        return true;
    }
    public async Task<Domain.Entities.Account> GetByIdAsync(int id)
    {
        return await _accountsDbContext.Account.AsTracking()
            .Include(a => a.Branch) 
            .ThenInclude(b => b.Location)
            .FirstOrDefaultAsync(a => a.Id == id && (a.IsDeleted == null || a.IsDeleted == false)) ?? throw new NotFoundException("Account Not Found");
    }
    public async Task UpdateAsync(Domain.Entities.Account account)
    {
        _accountsDbContext.Account.Entry(account).State = EntityState.Modified;
        await _accountsDbContext.SaveChangesAsync();
    }
}