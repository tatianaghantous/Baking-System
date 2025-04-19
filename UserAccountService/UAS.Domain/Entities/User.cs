using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UAS.Domain.Entities;

public partial class User
{
    public Guid Id { get; set; }
    [Required]
    public string Username { get; set; } = null!;

    [Required]
    public int? RoleId { get; set; }
    public int? BranchId { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<Account> Account { get; set; } = new List<Account>();

    public virtual Branch? Branch { get; set; }

    public virtual Role? Role { get; set; }
}
