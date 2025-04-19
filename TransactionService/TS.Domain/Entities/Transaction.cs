using System;
using System.Collections.Generic;

namespace TS.Domain.Entities;

public partial class Transaction
{
    public int Id { get; set; }

    public int? AccountId { get; set; }

    public decimal Amount { get; set; }

    public int? TransactionTypeId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CurrencyId { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Currency? Currency { get; set; }
    // Navigation property to Account
    public virtual Account? Account { get; set; }

    public virtual ICollection<RecurrentTransaction> RecurrentTransaction { get; set; } = new List<RecurrentTransaction>();

    public virtual Transactiontype? TransactionType { get; set; }
}
