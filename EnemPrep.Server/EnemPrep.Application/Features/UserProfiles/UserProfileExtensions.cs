using EnemPrep.Domain.Models;

namespace EnemPrep.Application.Features.UserProfiles;

public static class UserProfileExtensions
{
    public static GetUserProfile ToGetUserProfile(this UserProfile profile)
    {
        return new GetUserProfile
        {
            UserBio = profile.UserBio,
            StreakCount = profile.StreakCount,
            ExperiencePoints = profile.ExperiencePoints,
            CurrentLevel = profile.CurrentLevel,
            LastActivityDate = profile.LastActivityDate
        };
    }
}