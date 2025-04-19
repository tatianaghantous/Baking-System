namespace UAS.Domain.DTOs;

public class LocationDto
{
    public int Id { get; set; }

    public string LocationName { get; set; } = null!;

    public ICollection<BranchDto> Branches { get; set; } = new List<BranchDto>();
}