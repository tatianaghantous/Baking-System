using AutoMapper;
using Foxera.Common.CustomExceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UAS.Contracts.Persistence;
using UAS.Domain.DTOs;

namespace UAS.Application.Features.Account.Queries;

public class GetAccountByIdQuery: IRequest<AccountDto>
{
    public int AccountId { get; set; }
}

public class GetAccountByIdQueryHandler : IRequestHandler<GetAccountByIdQuery, AccountDto>
{
    private readonly IAccountsDbContext _accountsDbContext;
    private readonly IMapper _mapper;

    public GetAccountByIdQueryHandler(IAccountsDbContext accountsDbContext, IMapper mapper)
    {
        _accountsDbContext = accountsDbContext;
        _mapper = mapper;
    }

    public async Task<AccountDto> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
    {
        var account = await GetByIdAsync(request.AccountId);
        return _mapper.Map<AccountDto>(account);
    }
    public async Task<Domain.Entities.Account> GetByIdAsync(int id)
    {
        return await _accountsDbContext.Account
            .Include(a => a.Branch) 
            .ThenInclude(b => b.Location)
            .FirstOrDefaultAsync(a => a.Id == id && (a.IsDeleted == null || a.IsDeleted == false)) ?? throw new NotFoundException("Account Not Found");
    }

}