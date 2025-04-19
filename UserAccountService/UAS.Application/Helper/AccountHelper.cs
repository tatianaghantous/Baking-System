using Foxera.Keycloak.Contracts;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Errors.Model;
using UAS.Contracts.Persistence;
using UAS.Domain.Entities;

namespace UAS.Application.Helper;

public class AccountHelper
{
    private readonly IAccountsDbContext _context;
    private readonly ICurrentUser _currentUser;

    public AccountHelper(IAccountsDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Account>
        GetAsync(long accountId, CancellationToken cancellationToken) //used for editing an account
    {
        var desiredAccount = await _context.Account.SingleOrDefaultAsync(x => x.Id == accountId);

        if (desiredAccount == null)
            throw new NotFoundException();

        if (HasAccess(desiredAccount))
        {
            return desiredAccount;
        }
        else
        {
            throw new ForbiddenException("you're not authorized to access this account");
        }
    }


    public async Task<List<Account>> GetAsync(CancellationToken cancellationToken) //used for get queries
    {
        var filters = GetFilters();
        return await _context.Account.Where(filters).ToListAsync(cancellationToken);
    }

    public bool IsInRole(string roleName)
    {
        return _currentUser.Roles != null && _currentUser.Roles.Any(c => c == roleName);
    }

    private ExpressionStarter<Account> GetFilters()
    {
        var pd = PredicateBuilder.New<Account>(defaultExpression: true);

        //employee and admin has read access to all data of all branches, or the customer , only his accounts
        if (_currentUser.IsInRole("Customer"))
        {
            pd = pd.And(x => x.UserId == _currentUser.Id);
        }

        return pd;
    }

    private bool HasAccess(Account desiredAccount)
    {
        //customer can't edit his account

        //employee can edit only in his branch
        if (_currentUser.IsInRole("Employee") && desiredAccount.BranchId == _currentUser.BranchId)
        {
            return true;
        }
        else if (_currentUser.IsInRole("Admin")) //admin can edit in any branch
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}