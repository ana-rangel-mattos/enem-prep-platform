using EnemPrep.Persistence.Constants;

namespace EnemPrep.Domain.Result;

public static class QuestionErrors
{
    public static Error QuestionNotFound(Guid? id) => new Error(
        ErrorNames.QuestionNotFound,
        $"Question with ID {id} was not found."
    );

    public static Error QuestionNullId => new Error(
        ErrorNames.QuestionNullId,
        $"Question ID cannot be null."
    );

    public static Error NullPublisherId => new Error(
        ErrorNames.QuestionNullPublisherId,
        "Question publisher ID cannot be null."
    );

    public static Error PublisherIdDoesNotMatchLoggedUserId => new Error(
        ErrorNames.QuestionPublisherIsNotLoggedUser,
        "Question publisher ID does not match logged user ID."
    );
}