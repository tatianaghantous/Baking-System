using AutoMapper;
using UAS.Domain.DTOs;
using UAS.Domain.Entities;

namespace UAS.API.Configurations;

public class MappingProfile :  Profile
{
    public MappingProfile()
    {
        // Map User to UserDto
        CreateMap<User, UserDto>();

        // Map Role to RoleDto
        CreateMap<Role, RoleDto>();

        // Map Branch to BranchDto
        CreateMap<Branch, BranchDto>();

        // Map Account to AccountDto
        CreateMap<Account, AccountDto>();

        // Map Location to LocationDto
        CreateMap<Location, LocationDto>();
    }

}