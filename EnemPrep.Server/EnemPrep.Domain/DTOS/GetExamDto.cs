using EnemPrep.Domain.Enums;

namespace EnemPrep.Domain.DTOS;

public class GetExamDto
{
    public Guid ExamId { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string StatusDescription { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;
    public string LanguageDescription { get; set; } = string.Empty;
    public int ExamYear { get; set; }
    public int QuestionsCount { get; set; }
    public int? CorrectQuestionsCount { get; set; }
    public int? IncorrectQuestionsCount { get; set; }
    public int? UnsolvedQuestionsCount { get; set; }
    public TimeSpan? TotalSpentTime { get; set; }
    public TimeSpan MaxSpentTime { get; set; }
    public float? EstimatedScore { get; set; }
    public DateTime CreatedAt { get; set; }
}