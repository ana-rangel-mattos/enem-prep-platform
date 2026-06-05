using EnemPrep.Persistence.Constants;

namespace EnemPrep.Domain.Result;

public static class SavedQuestionErrors
{
    public static Error QuestionNotFound(Guid? questionId) => new Error(
        ErrorNames.SavedQuestionQuestionNotFound,
        $"Question with ID {questionId} could not be found.");
    
    public static Error SavedQuestionNotFound(Guid? questionId) => new Error(
        ErrorNames.SavedQuestionQuestionNotFound,
        $"Saved question with ID {questionId} could not be found.");
}