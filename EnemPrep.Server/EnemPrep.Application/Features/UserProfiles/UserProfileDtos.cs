namespace EnemPrep.Application.Features.UserProfiles;

public class GetUserProfile
{
    public string? UserBio { get; set; }
    public int StreakCount { get; set; }
    public int ExperiencePoints { get; set; }
    public int CurrentLevel { get; set; }
    public DateTime LastActivityDate { get; set; }
}

public class UpdateUserBio
{
    public string? UserBio { get; set; } = string.Empty;
}