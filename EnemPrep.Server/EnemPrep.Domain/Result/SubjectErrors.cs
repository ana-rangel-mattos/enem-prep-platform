using EnemPrep.Persistence.Constants;

namespace EnemPrep.Domain.Result;

public static class SubjectErrors
{
    public static Error SubjectNotFound(Guid? id) => new Error(
        ErrorNames.SubjectNotFound,
        $"Subject with ID {id} was not found."
    );

    public static Error SubjectNullId => new Error(
        ErrorNames.SubjectNullId,
        $"Subject ID cannot be null."
    );
}