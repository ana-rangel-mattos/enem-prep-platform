namespace EnemPrep.Domain.Result;

public class UserProfileErrors
{
    public static Error UserPreferenceWasNotFound => new Error(
        ErrorNames.UserProfileUserProfileWasNotFound,
        $"Could not find user profile.");
}