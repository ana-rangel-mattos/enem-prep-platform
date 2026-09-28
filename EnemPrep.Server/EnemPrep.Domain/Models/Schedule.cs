namespace EnemPrep.Domain.Models;

public partial class Schedule
{
    public Guid ScheduleId { get; set; }

    public Guid UserId { get; set; }

    public string Name { get; set; } = null!;
    public bool HasWeekEnd { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<ScheduleSubject> ScheduleSubjects { get; set; } = new List<ScheduleSubject>();

    public virtual User User { get; set; } = null!;
}
