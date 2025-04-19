using System;
using System.Collections.Generic;

namespace TS.Domain.Entities;

public partial class Account
{
    public int Id { get; set; }
    public Guid Guid { get; set; }
    public bool? IsDeleted { get; set; }

    public int? BranchId { get; set; }
    public virtual ICollection<Transaction> Transaction { get; set; } = new List<Transaction>();
}
