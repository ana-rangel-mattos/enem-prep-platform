using EnemPrep.Domain.Models;

namespace EnemPrep.Application.Features.SolvedQuestions;

public class PostSolvedQuestionDto
{
    public Guid QuestionId { get; set; }
    public int QuestionYear { get; set; }
    public char CorrectAlternative { get; set; }
    public char? ChosenAlternative { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime FinishedAt { get; set; }

    public SolvedQuestion ToSolvedQuestion()
    {
        return new SolvedQuestion
        {
            QuestionId = QuestionId,
            QuestionYear = QuestionYear,
            CorrectAlternative = CorrectAlternative,
            ChosenAlternative = ChosenAlternative,
            IsCorrect = ChosenAlternative == CorrectAlternative,
            TimeSpent = FinishedAt - StartedAt
        };
    }
}

public class GetSolvedQuestionDto
{
    public Guid SolvedQuestionId { get; init; }
    public Guid QuestionId { get; init; }
    public int QuestionYear { get; init; }
    public char CorrectAlternative { get;init; }
    public char? ChosenAlternative { get; init; }
    public string? QuestionText { get; init; } = null!;
    public string? CorrectAlternativeText { get; init; } = null!;
    public string? ChosenAlternativeText { get; init; }
    public TimeSpan TimeSpent { get; init; }
    public bool IsCorrect { get; init; }
    public DateTime CreatedAt { get; init; }
}