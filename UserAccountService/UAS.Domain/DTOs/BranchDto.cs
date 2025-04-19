namespace UAS.Domain.DTOs;

public class BranchDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? LocationId { get; set; }

    public ICollection<AccountDto> Accounts { get; set; } = new List<AccountDto>();

    public LocationDto? Location { get; set; }

    public ICollection<UserDto> Users { get; set; } = new List<UserDto>();
}