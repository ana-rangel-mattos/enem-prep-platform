using EnemPrep.Domain.Models;

namespace EnemPrep.Application.Features.UserGoals;

public class GetUserGoalDto
{
    public Guid UserId { get; set; }
    public Guid UserGoalId { get; set; }
    public string UniversityName { get; set; }
    public string CourseName { get; set; }
    public float CutOffScore { get; set; }
}

public class PostUserGoalDto
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

public class UpdateUserGoalDto
{
    public string UniversityName { get; set; }
    public string CourseName { get; set; }
    public float CutOffScore { get; set; }
}