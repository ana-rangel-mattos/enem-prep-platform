using EnemPrep.Domain.Enums;

namespace EnemPrep.Domain.Models;

public class GetScheduleSubjectDto
{
    public DayOfTheWeek Weekday { get; set; }
    public int OrderIndex { get; set; }
}