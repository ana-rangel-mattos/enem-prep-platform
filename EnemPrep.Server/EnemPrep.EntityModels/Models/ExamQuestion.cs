using System;
using System.Collections.Generic;

namespace EnemPrep.EntityModels.Models;

public partial class ExamQuestion
{
    public Guid ExamId { get; set; }

    public Guid QuestionId { get; set; }

    public char CorrectAlternative { get; set; }

    public char? ChosenAlternative { get; set; }

    public bool IsCorrect { get; set; }

    public TimeSpan TimeSpent { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Exam Exam { get; set; } = null!;

    public virtual Question Question { get; set; } = null!;
}
