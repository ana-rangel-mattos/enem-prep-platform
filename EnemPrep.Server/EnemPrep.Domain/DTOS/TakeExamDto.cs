namespace EnemPrep.Domain.DTOS;

public class TakeExamDto
{
    public Guid ExamId { get; set; }
    public string Title { get; set; } = string.Empty;
    public List<ExamQuestionDto> Questions = [];
}