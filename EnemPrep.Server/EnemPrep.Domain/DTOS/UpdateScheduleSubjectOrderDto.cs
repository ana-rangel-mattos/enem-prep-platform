using EnemPrep.Domain.Enums;

namespace EnemPrep.Domain.DTOS;

public class UpdateScheduleSubjectOrderDto
{
    public Guid SubjectId { get; set; }
    public DayOfTheWeek NewDay { get; set; }
    public int NewIndex { get; set; }
}