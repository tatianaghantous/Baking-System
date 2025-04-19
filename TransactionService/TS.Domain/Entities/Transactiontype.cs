using System;
using System.Collections.Generic;

namespace TS.Domain.Entities;

public partial class Transactiontype
{
    public int Id { get; set; }

    public string TypeName { get; set; } = null!;

    public virtual ICollection<Transaction> Transaction { get; set; } = new List<Transaction>();
}
