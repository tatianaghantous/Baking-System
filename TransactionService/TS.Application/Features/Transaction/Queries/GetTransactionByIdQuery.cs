using AutoMapper;
using Foxera.Common.CustomExceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Contracts.Persistence;
using TS.Domain.DTOs;

namespace TS.Persistence.Features.Transaction.Queries;

public class GetTransactionByIdQuery : IRequest<TransactionDto>
{
    public int TransactionId { get; set; }
}
public class GetTransactionByIdQueryHandler : IRequestHandler<GetTransactionByIdQuery, TransactionDto>
{
    private readonly ITransactionsDbContext _transactionsDbContext;
    private readonly IMapper _mapper;

    public GetTransactionByIdQueryHandler(ITransactionsDbContext transactionsDbContext, IMapper mapper)
    {
        _transactionsDbContext = transactionsDbContext;
        _mapper = mapper;
    }

    public async Task<TransactionDto> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
    {

        var transaction = await _transactionsDbContext.Transaction
            .Include(t => t.Currency)
            .Include(t => t.TransactionType)
            .Include(t => t.RecurrentTransaction)
            .SingleOrDefaultAsync(t => t.Id == request.TransactionId, cancellationToken);
            
        if (transaction == null)
        {
            throw new NotFoundException("Transaction Not Found"); 
        }
        
        var transactionDto = _mapper.Map<TransactionDto>(transaction);
        return transactionDto;
    }
}