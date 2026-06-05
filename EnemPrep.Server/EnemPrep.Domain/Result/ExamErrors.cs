using EnemPrep.Persistence.Constants;

namespace EnemPrep.Domain.Result;

public static class ExamErrors
{
    public static Error InvalidSubjects => new Error(
        ErrorNames.ExamInvalidSubjects,
        "An exam must have at least one subject.");

    public static Error NoEnoughQuestionsForSubject(string subjectName) => new Error(
        ErrorNames.ExamNoEnoughQuestionsForSubject,
        $"There are not enough questions for the subject {subjectName}.");
    
    public static Error ExamWasNotFound(Guid? examId) => new Error(
        ErrorNames.ExamExamWasNotFound,
        $"Exam with ID {examId} was not found.");
    
    public static Error InvalidNewStatus => new Error(
        ErrorNames.ExamInvalidNewStatus,
        "The new status should be one step ahead of the previous one or canceled.");

    public static Error FailedToUpdateExamStatus => new Error(
        ErrorNames.ExamFailedToUpdateExamStatus,
        "Could not update exam status successfully.");

    public static Error ExamDoesNotBelongToLoggedUser => new Error(
        ErrorNames.ExamExamDoesNotBelongToLoggedUser,
        "Exam does not belong to the logged user.");

    public static Error ExamQuestionWasNotFound(Guid examQuestionId) => new Error(
        ErrorNames.ExamExamQuestionWasNotFound,
        $"Could not find exam question with ID {examQuestionId}.");
    
    public static Error FailedToSubmitQuestionAnswer => new Error(
        ErrorNames.ExamFailedToSubmitQuestionAnswer,
        "Could not submit question answer successfully.");

    public static Error FailedToCompleteExam(Guid examId) => new Error(
        ErrorNames.ExamFailedToCompleteExam,
        $"Could not complete exam with ID {examId} successfully.");
}