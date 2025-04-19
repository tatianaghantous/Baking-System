using System.Net.Http.Json;
using System.Text;
using AutoMapper;
using Foxera.Common.CustomExceptions;
using Foxera.Mail.Interface;
using Foxera.Mail.Models;
using Foxera.RabitMq;
using Hangfire;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RabbitMQ.Client;
using UAS.Contracts.Persistence;
using UAS.Domain.DTOs;

namespace UAS.Application.Features.Account.Commands;

public class CreateAccountCommand: IRequest<AccountDto>
{
    public Guid UserId { get; set; }
    public int BranchId { get; set; }
    public decimal InitialBalance { get; set; }
}

public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, AccountDto>
{
    private readonly IAccountsDbContext _accountsDbContext;
    private readonly IMapper _mapper;
    private readonly IRabbitMQService _rabbitMQService;

    public CreateAccountCommandHandler(IAccountsDbContext accountsDbContext, IMapper mapper, IRabbitMQService rabbitMQService)
    {
        _accountsDbContext = accountsDbContext;
        _mapper = mapper;
        _rabbitMQService = rabbitMQService;
    }
    

    public async Task<AccountDto> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        // Check if user exists
        var user = await GetByIdAsync(request.UserId);
        if (user == null || user.IsDeleted == true)
        {
            throw new KeyNotFoundException("User not found.");
        }

        // Check if the user already has 5 accounts across all branches
        var accountCount = await GetUserAccountCountAsync(request.UserId);
        if (accountCount >= 5)
        {
            throw new InvalidOperationException("User cannot have more than 5 accounts.");
        }

        // Create the account
        var account = new Domain.Entities.Account
        {
            UserId = request.UserId,
            BranchId = request.BranchId,
            Balance = request.InitialBalance,
            CreatedAt = DateTime.Now,
            IsDeleted = false
        };

        await _accountsDbContext.Account.AddAsync(account);
        await _accountsDbContext.SaveChangesAsync();
        
        _rabbitMQService.Send("account-created",account);
        // Map to AccountDto
        var accountDto = _mapper.Map<AccountDto>(account);

        return accountDto;
    }
    public async Task<Domain.Entities.User> GetByIdAsync(Guid id)
    {
        return await _accountsDbContext.User
            .Include(a => a.Account) 
            .FirstOrDefaultAsync(a => a.Id == id && (a.IsDeleted == null || a.IsDeleted == false)) ?? throw new NotFoundException("User Not Found");
    }
    public async Task<int> GetUserAccountCountAsync(Guid userId)
    {
        return await _accountsDbContext.Account
            .Where(a => a.UserId == userId && (a.IsDeleted == null || a.IsDeleted == false))
            .CountAsync();
    }
}