using System.Text.Json;
using EnemPrep.Domain.Enums;
using EnemPrep.Domain.Models;

namespace EnemPrep.Application.Features.Questions;

public class QuestionDto
{
    public Guid QuestionId { get; init; }
    public int? ApiIndex { get; init; }
    public JsonElement Content { get; init; }
    public Guid? SubjectId { get; init; }
    public DateTime? CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
}

public class PostQuestionDto
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

public class PostSavedQuestionDto
{
    public Guid QuestionId { get; set; }
    public string? Notes { get; set; }
}

public class GetSavedQuestionDto
{
    public Guid SavedQuestionId { get; set; }
    public Guid QuestionId { get; set; }
    public string? Notes { get; set; }
    public string? QuestionText { get; set; }
}