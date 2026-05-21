using EnemPrep.Persistence.Constants;

namespace EnemPrep.Domain.Result;

public static class UserErrors
{
    public static Error UserNotFound(Guid? id) => new Error(
        ErrorNames.UserUserNotFound,
        $"User with ID {id} was not found."
    );

    public static Error UserNullId => new Error(
        ErrorNames.UserNullUserId,
        $"User ID cannot be null."
    );
}