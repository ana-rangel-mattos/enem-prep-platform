namespace EnemPrep.Domain.DTOS;

public class GetUserProfile
{
    public string? UserBio { get; set; }
    public int StreakCount { get; set; }
    public int ExperiencePoints { get; set; }
    public int CurrentLevel { get; set; }
    public DateTime LastActivityDate { get; set; }
}