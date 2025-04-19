namespace TS.Domain.DTOs;

public class RecurrentTransactionDto
{
    public int Id { get; set; }

    public int? TransactionId { get; set; }

    public int? BgJobId { get; set; }
}