using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using EnemPrep.Domain.Enums;

namespace EnemPrep.Domain.Models;

public partial class Question
{
    public Guid QuestionId { get; set; }

    public Guid? SubjectId { get; set; }

    public Guid PostedById { get; set; }

    public int ApiIndex { get; set; }
    
    [Column("language")]

    public Language? Language { get; set; } = null;

    public string Content { get; set; } = null!;

    public DateTime UpdatedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<ExamQuestion> ExamQuestions { get; set; } = new List<ExamQuestion>();

    public virtual User PostedBy { get; set; } = null!;

    public virtual ICollection<SolvedQuestion> SolvedQuestions { get; set; } = new List<SolvedQuestion>();

    public virtual Subject? Subject { get; set; }
}
