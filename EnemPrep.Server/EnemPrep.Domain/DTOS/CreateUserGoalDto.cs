using EnemPrep.Domain.Models;

namespace EnemPrep.Domain.DTOS;

public class CreateUserGoalDto
{
    public string UniversityName { get; set; }
    public string CourseName { get; set; }
    public float CutOffScore { get; set; }

    public UserGoal ToUserGoal()
    {
        return new UserGoal
        {
            CourseName = CourseName,
            UniversityName = UniversityName,
            CutOffScore = CutOffScore,
            UserGoalId = Guid.CreateVersion7(),
        };
    }
}