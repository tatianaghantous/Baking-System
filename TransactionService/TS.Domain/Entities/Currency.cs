using System;
using System.Collections.Generic;

namespace TS.Domain.Entities;

public partial class Currency
{
    public int Id { get; set; }

    public string CurrencyCode { get; set; } = null!;

    public string CurrencyName { get; set; } = null!;

    public virtual ICollection<Transaction> Transaction { get; set; } = new List<Transaction>();
}
