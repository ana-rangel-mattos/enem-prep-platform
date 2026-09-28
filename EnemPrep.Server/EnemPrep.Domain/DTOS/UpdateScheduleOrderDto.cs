using EnemPrep.Domain.Enums;

namespace EnemPrep.Domain.DTOS;

public class UpdateScheduleOrderDto
{
    public DayOfTheWeek DayOfWeek { get; set; }
    public ICollection<UpdateScheduleSubjectDto> Items { get; set; } = new List<UpdateScheduleSubjectDto>();
}