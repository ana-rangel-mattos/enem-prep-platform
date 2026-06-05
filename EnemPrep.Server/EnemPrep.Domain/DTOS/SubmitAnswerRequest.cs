namespace EnemPrep.Domain.DTOS;

public class SubmitAnswerRequest
{
    public Guid QuestionId { get; set; }
    public Guid ExamId { get; set; }
    public char ChosenAlternative { get; set; }
    public int TimeSpentInSeconds { get; set; }
}