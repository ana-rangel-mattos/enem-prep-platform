namespace EnemPrep.Domain.DTOS;

public class UpdateScheduleSubjectDto
{
    public Guid ScheduleId { get; set; }
    public Guid SubjectId { get; set; }
    public int OrderIndex { get; set; }
}