using System;
using System.Collections.Generic;

namespace EnemPrep.Domain.Models;

public partial class Schedule
{
    public Guid ScheduleId { get; set; }

    public Guid UserId { get; set; }

    public string Name { get; set; } = null!;

    public DateTime UpdatedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<ScheduleSubject> ScheduleSubjects { get; set; } = new List<ScheduleSubject>();

    public virtual User User { get; set; } = null!;
}
