namespace TS.Domain.DTOs;

public class AccountDto
{
    public int Id { get; set; }

    public bool? IsDeleted { get; set; }

    public int? BranchId { get; set; }
}