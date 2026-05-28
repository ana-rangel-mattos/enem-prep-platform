namespace EnemPrep.Domain.DTOS;

public class GetSavedQuestionDto
{
    public Guid SavedQuestionId { get; set; }
    public Guid QuestionId { get; set; }
    public string? Notes { get; set; }
    public string? QuestionText { get; set; }
}