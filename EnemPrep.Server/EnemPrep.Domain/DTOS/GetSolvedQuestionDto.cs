using EnemPrep.Domain.Models;

namespace EnemPrep.Domain.DTOS;

public class GetSolvedQuestionDto
{
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