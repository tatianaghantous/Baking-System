using System;
using System.Collections.Generic;

namespace TS.Domain.Entities;

public partial class RecurrentTransaction
{
    public int Id { get; set; }

    public int? TransactionId { get; set; }
    public DateTime? NextExecutionDate { get; set; }
    public int PeriodTypeId { get; set; }
    public virtual PeriodType PeriodType { get; set; } 
    public virtual Transaction? Transaction { get; set; }
}
