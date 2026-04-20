using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
    public ExamStatus LanguageChoice { get; set; } = ExamStatus.NotStarted;

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
}
