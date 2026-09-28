using EnemPrep.Domain.Enums;

namespace EnemPrep.Domain.DTOS;

public class UpdateScheduleSubjectWeekDayDto
{
    public Guid SubjectId { get; set; }
    public DayOfTheWeek NewDay { get; set; }
    public DayOfTheWeek PreviousDay { get; set; }
    public int OrderIndex { get; set; }
}