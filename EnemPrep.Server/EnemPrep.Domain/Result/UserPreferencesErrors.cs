using EnemPrep.Persistence.Constants;

namespace EnemPrep.Domain.Result;

public static class UserPreferencesErrors
{
    public static Error UserPreferenceWasNotFound(Guid userId) => new Error(
        ErrorNames.UserPreferencesUserPreferenceWasNotFound,
        $"Could not find user preferences for user with ID {userId}.");
    
    public static Error FailedToUpdateUserPreferences => new Error(
        ErrorNames.UserPreferencesFailedToUpdateUserPreferences,
        "Failed to update user preferences.");
    
    public static Error FailedToResetUserPreferences => new Error(
        ErrorNames.UserPreferencesFailedToResetUserPreferences,
        "Failed to reset user preferences.");
}