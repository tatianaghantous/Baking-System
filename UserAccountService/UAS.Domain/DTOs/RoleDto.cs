namespace UAS.Domain.DTOs;

public class RoleDto
{
    public int Id { get; set; }

    public string RoleName { get; set; } = null!;
    public ICollection<UserDto> Users { get; set; } = new List<UserDto>();
}