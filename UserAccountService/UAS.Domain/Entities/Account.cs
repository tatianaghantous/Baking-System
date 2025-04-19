using System;
using System.Collections.Generic;

namespace UAS.Domain.Entities;

public partial class Account
{
    public int Id { get; set; }

    public Guid? UserId { get; set; }

    public int? BranchId { get; set; }

    public decimal Balance { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Branch? Branch { get; set; }

    public virtual User? User { get; set; }
}
