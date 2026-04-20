using System;
using System.Collections.Generic;
using EnemPrep.Domain.Enums;

namespace EnemPrep.Domain.Models;

public partial class ScheduleSubject
{
    public Guid ScheduleId { get; set; }

    public Guid SubjectId { get; set; }
    
    public DayOfTheWeek Weekday { get; set; }

    public int SubjectOrder { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Schedule Schedule { get; set; } = null!;

    public virtual Subject Subject { get; set; } = null!;
}
