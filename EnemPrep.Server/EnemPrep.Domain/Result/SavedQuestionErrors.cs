using EnemPrep.Persistence.Constants;

namespace EnemPrep.Domain.Result;

public static class SavedQuestionErrors
{
    public static Error UserIsNotLoggedIn => new Error(
        ErrorNames.SavedQuestionUserIsNotLoggedIn,
        $"User must be logged in to save questions.");

    public static Error QuestionNotFound(Guid? questionId) => new Error(
        ErrorNames.SavedQuestionQuestionNotFound,
        $"Question with ID {questionId} could not be found.");
    
    public static Error SavedQuestionNotFound(Guid? questionId) => new Error(
        ErrorNames.SavedQuestionQuestionNotFound,
        $"Saved question with ID {questionId} could not be found.");
}