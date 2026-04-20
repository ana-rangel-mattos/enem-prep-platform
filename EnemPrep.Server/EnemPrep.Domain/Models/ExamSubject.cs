using System;
using System.Collections.Generic;

namespace EnemPrep.Domain.Models;

public partial class ExamSubject
{
    public Guid SubjectId { get; set; }

    public Guid ExamId { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Exam Exam { get; set; } = null!;

    public virtual Subject Subject { get; set; } = null!;
}
