using System;
using System.Collections.Generic;

namespace EnemPrep.Domain.Models;

public partial class SolvedQuestion
{
    public Guid SolvedQuestionId { get; set; }

    public Guid UserId { get; set; }

    public Guid QuestionId { get; set; }

    public int QuestionYear { get; set; }

    public char CorrectAlternative { get; set; }

    public char? ChosenAlternative { get; set; }

    public bool IsCorrect { get; set; }

    public TimeSpan TimeSpent { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Question Question { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
