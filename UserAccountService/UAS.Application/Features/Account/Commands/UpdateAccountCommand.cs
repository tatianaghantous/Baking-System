using MediatR;
using UAS.Application.Helper;
using UAS.Contracts.Persistence;

namespace UAS.Application.Features.Account.Commands;


public class UpdateAccountCommand : IRequest<bool>
{
    public long Id { get; set; }
    public Guid UserId { get; set; }
    public long BranchId { get; set; }
    public decimal Balance { get; set; }
    public bool IsDeleted { get; set; }
}

public class UpdateAccountCommandHandler : IRequestHandler<UpdateAccountCommand, bool>
{
    private readonly IAccountsDbContext _context;
    private readonly AccountHelper _accountHelper;

    public UpdateAccountCommandHandler(IAccountsDbContext context, AccountHelper accountHelper)
    {
        _context = context;
        _accountHelper = accountHelper;
    }

    public async Task<bool> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
    {

        Domain.Entities.Account account =
            await _accountHelper.GetAsync(request.Id, cancellationToken); //at this lvl will do the filtering

        account.UserId = request.UserId;
        account.BranchId = (int?)request.BranchId;
        account.Balance = request.Balance;
        account.IsDeleted = request.IsDeleted;

        _context.Account.Update(account);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}