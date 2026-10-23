using EnemPrep.Domain.Enums;
using EnemPrep.Domain.Models;

namespace EnemPrep.Domain.DTOS;

public class GetScheduleDetailsDto
{
    public Guid ScheduleId { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool HasWeekEnd { get; set; }
    public Dictionary<DayOfTheWeek, IReadOnlyCollection<GetScheduleSubjectDto>> Schedule { get; set; } = new();
}
