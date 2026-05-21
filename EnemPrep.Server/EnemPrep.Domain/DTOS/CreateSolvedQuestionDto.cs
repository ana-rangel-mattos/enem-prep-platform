using EnemPrep.Domain.Models;

namespace EnemPrep.Domain.DTOS;

public class CreateSolvedQuestionDto
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