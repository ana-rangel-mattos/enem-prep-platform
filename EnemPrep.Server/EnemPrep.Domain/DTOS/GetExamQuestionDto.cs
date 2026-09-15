using System.Text.Json;

namespace EnemPrep.Domain.DTOS;

public class GetExamQuestionDto
{
    public Guid ExamId { get; set; }
    public Guid QuestionId { get; init; }
    public JsonElement Content { get; init; }
}