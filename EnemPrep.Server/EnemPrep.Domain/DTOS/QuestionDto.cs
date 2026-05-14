using System.Text.Json;

namespace EnemPrep.Domain.DTOS;

public class QuestionDto
{
    public Guid QuestionId { get; init; }
    public int? ApiIndex { get; init; }
    public JsonElement Content { get; init; }
    public Guid? SubjectId { get; init; }
    public DateTime? CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
}