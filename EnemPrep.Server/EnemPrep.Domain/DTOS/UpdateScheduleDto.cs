namespace EnemPrep.Domain.DTOS;

public class UpdateScheduleDto
{
    public string Name { get; set; } = string.Empty;
    public bool HasWeekEnd { get; set; }
}