namespace TS.Domain.DTOs;

public class TransactionDto
{
    public int Id { get; set; }

    public int? AccountId { get; set; }

    public decimal Amount { get; set; }

    public int? TransactionTypeId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CurrencyId { get; set; }

    public bool? IsDeleted { get; set; }

    public CurrencyDto? Currency { get; set; }

    public ICollection<RecurrentTransactionDto> RecurrentTransaction { get; set; } = new List<RecurrentTransactionDto>();

    public TransactionTypeDto? TransactionType { get; set; }
}