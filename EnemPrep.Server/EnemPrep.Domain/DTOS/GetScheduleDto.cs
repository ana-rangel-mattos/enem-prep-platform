namespace EnemPrep.Domain.DTOS;

public class GetScheduleDto
{
    public Guid ScheduleId { get; set; }

    public string Name { get; set; } = string.Empty;
}