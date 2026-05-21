using System.Text.Json;
using EnemPrep.Domain.Enums;
using EnemPrep.Domain.Models;

namespace EnemPrep.Domain.DTOS;

public class CreateQuestionDto
{
    public int? ApiIndex { get; init; }
    public JsonElement Content { get; init; }
    public Language? Language { get; init; }
    public Guid? SubjectId { get; init; }
    public Guid? UploadedById { get; init; }

    public Question ToQuestion()
    {
        return new Question
        {
            ApiIndex = ApiIndex ?? 0,
            Language = Language,
            SubjectId = SubjectId,
            Content = Content.ToString(),
        };
    }
}