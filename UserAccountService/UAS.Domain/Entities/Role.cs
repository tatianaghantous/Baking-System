using System;
using System.Collections.Generic;

namespace UAS.Domain.Entities;

public partial class Role
{
    public int Id { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<User> User { get; set; } = new List<User>();
}
