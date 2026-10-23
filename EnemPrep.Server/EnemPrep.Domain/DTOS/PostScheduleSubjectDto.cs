using EnemPrep.Domain.Enums;

namespace EnemPrep.Domain.DTOS;

public class PostScheduleSubjectDto
{
    public Guid SubjectId { get; set; }
    public DayOfTheWeek WeekDay { get; set; }
}