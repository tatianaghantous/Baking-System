namespace UAS.Domain.DTOs;

public class UserDto
{
    public Guid Id { get; set; }

    public string Username { get; set; } = null!;

    public int? RoleId { get; set; }

    public int? BranchId { get; set; }

    public bool? IsDeleted { get; set; }

    public ICollection<AccountDto> Accounts { get; set; } = new List<AccountDto>();

    public BranchDto? Branch { get; set; }

    public RoleDto? Role { get; set; }
}