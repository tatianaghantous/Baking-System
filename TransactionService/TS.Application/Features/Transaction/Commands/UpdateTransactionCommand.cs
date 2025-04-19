using AutoMapper;
using Foxera.Common.CustomExceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Contracts.Persistence;
using TS.Domain.DTOs;
using TS.Persistence.Helper;

namespace TS.Persistence.Features.Transaction.Commands;

public class UpdateTransactionCommand : IRequest<TransactionDto>
{
    public int Id { get; set; }
    public int? AccountId { get; set; }
    public decimal Amount { get; set; }
    public int? TransactionTypeId { get; set; }
    public DateTime? CreatedAt { get; set; }
    public int? CurrencyId { get; set; }
    public bool? IsDeleted { get; set; }
}
public class UpdateTransactionCommandHandler : IRequestHandler<UpdateTransactionCommand, TransactionDto>
{
    private readonly ITransactionsDbContext _transactionsDbContext;
    private readonly IMapper _mapper;
    private readonly AccountHelper _accountHelper;
    private readonly IMediator _mediator;
    public UpdateTransactionCommandHandler(ITransactionsDbContext transactionsDbContext, IMapper mapper, AccountHelper accountHelper, IMediator mediator)
    {
        _transactionsDbContext = transactionsDbContext;
        _mapper = mapper;
        _accountHelper = accountHelper;
        _mediator = mediator;
    }

    public async Task<TransactionDto> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = await _transactionsDbContext.Transaction
            .SingleOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (transaction == null)
        {
            throw new NotFoundException("Transaction Not Found"); 
        }
        // // grpc to get the account
        // if (!_accountHelper.HasAccess(account))
        // {
        //     throw new UnauthorizedAccessException();
        // }
        transaction.AccountId = request.AccountId;
        transaction.Amount = request.Amount;
        transaction.TransactionTypeId = request.TransactionTypeId;
        transaction.CreatedAt = request.CreatedAt;
        transaction.CurrencyId = request.CurrencyId;
        transaction.IsDeleted = request.IsDeleted;

        _transactionsDbContext.Transaction.Update(transaction);
        await _transactionsDbContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<TransactionDto>(transaction);
    }
}