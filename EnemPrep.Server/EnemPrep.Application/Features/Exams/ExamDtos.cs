using System.Text.Json;
using EnemPrep.Domain.Enums;

namespace EnemPrep.Application.Features.Exams;

public class PostExamDto
{
    private const double TimeSpentForQuestionMinutes = 3.5;
    
    public int TotalQuestions { get; set; }
    public List<SubjectName> Areas { get; set; } = new List<SubjectName>();
    public Language? ExamLanguage { get; set; }

    public TimeSpan GetMaxSpentTime()
    {
        double totalMinutes = TimeSpentForQuestionMinutes * TotalQuestions;
        
        return TimeSpan.FromMinutes(totalMinutes);
    }
}

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

public class UpdateExamStatusDto
{
    public Guid? ExamId { get; set; }
    public ExamStatus NewStatus { get; set; }
}

public class GetExamQuestionDto
{
    public Guid ExamId { get; set; }
    public Guid QuestionId { get; init; }
    public JsonElement Content { get; init; }
}

public class TakeExamDto
{
    public Guid ExamId { get; set; }
    public string Title { get; set; } = string.Empty;
    public List<GetExamQuestionDto> Questions { get; set; }= [];
    public int MaxSpentTime { get; set; }
}

public class SubmitAnswerRequest
{
    public Guid QuestionId { get; set; }
    public Guid ExamId { get; set; }
    public char? ChosenAlternative { get; set; }
    public int TimeSpentInSeconds { get; set; }
}