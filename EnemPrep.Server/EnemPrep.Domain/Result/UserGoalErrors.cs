using EnemPrep.Persistence.Constants;

namespace EnemPrep.Domain.Result;

public static class UserGoalErrors
{
    public static Error NullUserId => new Error(
        ErrorNames.UserGoalNullUserId,
        "User must be logged.");

    public static Error UserGoalNotFound => new Error(
        ErrorNames.UserGoalNotFound,
        "User goal does not exist.");

    public static Error UserGoalAlreadyExists => new Error(
        ErrorNames.UserGoalAlreadyExists,
        "User goal already exists.");

    public static Error UserNotFound(Guid? userId) => new Error(
        ErrorNames.UserGoalUserNotFound,
        $"User with ID {userId} was not found.");
}