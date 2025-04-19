using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Contracts.Persistence;
using TS.Domain.DTOs;

namespace TS.Persistence.Features.Transaction.Queries;

public class GetTransactionsByAccountQuery : IRequest<IEnumerable<TransactionDto>>
{
    public int AccountId { get; set; }
}
public class GetTransactionsByAccountQueryHandler : IRequestHandler<GetTransactionsByAccountQuery, IEnumerable<TransactionDto>>
{
    private readonly ITransactionsDbContext _transactionsDbContext;
    private readonly IMapper _mapper;

    public GetTransactionsByAccountQueryHandler(ITransactionsDbContext transactionsDbContext, IMapper mapper)
    {
        _transactionsDbContext = transactionsDbContext;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TransactionDto>> Handle(GetTransactionsByAccountQuery request, CancellationToken cancellationToken)
    {
        var transactions = await _transactionsDbContext.Transaction
            .Where(t => t.AccountId == request.AccountId && t.IsDeleted == false)
            .Include(t => t.Currency)
            .Include(t => t.TransactionType)
            .Include(t => t.RecurrentTransaction)
            .ToListAsync(cancellationToken);

        return _mapper.Map<IEnumerable<TransactionDto>>(transactions);
    }
}