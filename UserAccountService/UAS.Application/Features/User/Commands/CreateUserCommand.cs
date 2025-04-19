using AutoMapper;
using Foxera.Keycloak.Contracts;
using MediatR;
using UAS.Contracts.Persistence;
using UAS.Domain.DTOs;

namespace UAS.Application.Features.User.Commands;

public class CreateUserCommand : IRequest<UserDto>
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public int? RoleId { get; set; }
    public int? BranchId { get; set; }
}
public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand,UserDto>
{
    private readonly IAccountsDbContext _accountsDbContext;
    private readonly IMapper _mapper;
    private ICurrentUser CurrentUser;
    public CreateUserCommandHandler(IAccountsDbContext accountsDbContext, IMapper mapper, ICurrentUser currentUser)
    {
        _accountsDbContext = accountsDbContext;
        _mapper = mapper;
        CurrentUser = currentUser;
    }

    public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var currentu = CurrentUser;
        var user = new Domain.Entities.User
        {
            Username = request.Username,
            RoleId = request.RoleId,
            BranchId = request.BranchId,
            IsDeleted = false
        };

        await _accountsDbContext.User.AddAsync(user);
        await _accountsDbContext.SaveChangesAsync();

        var userDto = _mapper.Map<UserDto>(user);
        return userDto;
    }
}