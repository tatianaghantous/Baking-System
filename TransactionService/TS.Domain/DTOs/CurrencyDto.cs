namespace TS.Domain.DTOs;

public class CurrencyDto
{
    public int Id { get; set; }

    public string CurrencyCode { get; set; } = null!;

    public string CurrencyName { get; set; } = null!; 
}