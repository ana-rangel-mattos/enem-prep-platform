using EnemPrep.Persistence.Constants;

namespace EnemPrep.Domain.Result;

public static class SolvedQuestionErrors
{
    public static Error SolvedQuestionNotFound(Guid? id) => new Error(
        ErrorNames.SolvedQuestionNotFound,
        $"Solved Question with ID {id} was not found."
    );
    
    public static Error SolvedQuestionQuestionNotFound(Guid? id) => new Error(
        ErrorNames.SolvedQuestionQuestionNotFound,
        $"Cannot solve question with ID {id} because it was not found."
    );

    public static Error SolvedQuestionNullId => new Error(
        ErrorNames.SolvedQuestionNullId,
        $"Solved Question ID cannot be null."
    );
    
    public static Error SolvedQuestionNullUserId => new Error(
        ErrorNames.SolvedQuestionNullUserId,
        $"Solved Question User ID cannot be null."
    );

    public static Error SolvedQuestionPrivateUser => new Error(
        ErrorNames.SolvedQuestionPrivateUser,
        "Private user."
    );
    
    public static Error SolvedQuestionUserDoesNotExist(Guid? id) => new Error(
        ErrorNames.SolvedQuestionInexistentUser,
        $"The user with ID {id} does not exist."
    );
}