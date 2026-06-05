using System.ComponentModel.DataAnnotations.Schema;
using EnemPrep.Domain.DTOS;
using EnemPrep.Domain.Enums;

namespace EnemPrep.Domain.Models;

public partial class Exam
{
    public Guid ExamId { get; set; }

    public Guid UserId { get; set; }

    public string Title { get; set; } = null!;
    
    [Column("status")]
    public ExamStatus Status { get; set; } = ExamStatus.NotStarted;

    [Column("language_choice")] 
    public Language LanguageChoice { get; set; } = Language.English;

    public int ExamYear { get; set; }

    public int QuestionsCount { get; set; }

    public int? CorrectQuestionsCount { get; set; }

    public int? IncorrectQuestionsCount { get; set; }

    public int? UnsolvedQuestionsCount { get; set; }

    public TimeSpan? TotalSpentTime { get; set; }

    public TimeSpan MaxSpentTime { get; set; }

    public float? EstimatedScore { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<ExamQuestion> ExamQuestions { get; set; } = new List<ExamQuestion>();

    public virtual ICollection<ExamSubject> ExamSubjects { get; set; } = new List<ExamSubject>();

    public virtual User User { get; set; } = null!;

    public GetExamDto ConvertToDto()
    {
        return new GetExamDto
        {
            ExamId = ExamId,
            UserId = UserId,
            ExamYear = ExamYear,
            Title = Title,
            Status = Status.ToString(),
            StatusDescription = Status switch
            {
                ExamStatus.NotStarted => "Not Started",
                ExamStatus.InProgress => "Em Progresso",
                ExamStatus.Finished => "Finalizado",
                ExamStatus.Canceled => "Cancelado",
                _ => "Desconhecido"
            },
            Language = LanguageChoice.ToString(),
            LanguageDescription = LanguageChoice switch
            {
                Language.English => "Inglês",
                Language.Spanish => "Espanhol",
                _ => "Não Informado"
            },
            QuestionsCount = QuestionsCount,
            CorrectQuestionsCount = CorrectQuestionsCount,
            IncorrectQuestionsCount = IncorrectQuestionsCount,
            UnsolvedQuestionsCount = UnsolvedQuestionsCount,
            TotalSpentTime = TotalSpentTime,
            MaxSpentTime = MaxSpentTime,
            EstimatedScore = EstimatedScore,
            CreatedAt = CreatedAt
        };
    }
}
