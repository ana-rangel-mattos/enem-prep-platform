using EnemPrep.Domain.Enums;

namespace EnemPrep.Domain.DTOS;

public class GetScheduleSubjectDto
{
    public DayOfTheWeek Weekday { get; set; }
    public int OrderIndex { get; set; }
}