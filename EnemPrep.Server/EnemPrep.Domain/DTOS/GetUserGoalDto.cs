namespace EnemPrep.Domain.DTOS;

public class GetUserGoalDto
{
    public Guid UserId { get; set; }
    public Guid UserGoalId { get; set; }
    public string UniversityName { get; set; }
    public string CourseName { get; set; }
    public float CutOffScore { get; set; }
}