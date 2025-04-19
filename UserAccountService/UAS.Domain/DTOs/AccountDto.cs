namespace UAS.Domain.DTOs;

public class AccountDto
{
    public int Id { get; set; }

    public Guid? UserId { get; set; }

    public int? BranchId { get; set; }

    public decimal Balance { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public BranchDto? Branch { get; set; }

    public UserDto? User { get; set; }
}