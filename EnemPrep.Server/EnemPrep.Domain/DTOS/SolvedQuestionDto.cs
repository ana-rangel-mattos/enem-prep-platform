using EnemPrep.Domain.Models;

namespace EnemPrep.Domain.DTOS;

public class SolvedQuestionDto
{
    public Guid QuestionId { get; init; }
    public int QuestionYear { get; init; }
    public char CorrectAlternative { get;init; }
    public char? ChosenAlternative { get; init; }
    public TimeSpan TimeSpent { get; init; }
    public bool IsCorrect { get; init; }
    public DateTime CreatedAt { get; init; }
}