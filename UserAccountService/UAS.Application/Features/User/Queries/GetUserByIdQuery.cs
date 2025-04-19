using AutoMapper;
using Foxera.Common.CustomExceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UAS.Contracts.Persistence;
using UAS.Domain.DTOs;

namespace UAS.Application.Features.User.Queries;

public class GetUserByIdQuery : IRequest<UserDto>
{
    public Guid UserId { get; set; }
}
public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
{
    private readonly IAccountsDbContext _accountsDbContext;
    private readonly IMapper _mapper;

    public GetUserByIdQueryHandler(IAccountsDbContext accountsDbContext, IMapper mapper)
    {
        _accountsDbContext = accountsDbContext;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await GetByIdAsync(request.UserId);

        if (user == null)
        {
            throw new NotFoundException("User Not Found");
        }

        var userDto = _mapper.Map<UserDto>(user);
        return userDto;
    }
    public async Task<Domain.Entities.User?> GetByIdAsync(Guid userId)
    {
        return await _accountsDbContext.User
            .Include(u => u.Role)
            .Include(u => u.Branch)
            .ThenInclude(b => b.Location)
            .FirstOrDefaultAsync(u => u.Id == userId);
    }
}