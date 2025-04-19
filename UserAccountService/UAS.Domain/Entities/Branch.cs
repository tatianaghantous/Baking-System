using System;
using System.Collections.Generic;

namespace UAS.Domain.Entities;

public partial class Branch
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? LocationId { get; set; }

    public virtual ICollection<Account> Account { get; set; } = new List<Account>();

    public virtual Location? Location { get; set; }

    public virtual ICollection<User> User { get; set; } = new List<User>();
}
