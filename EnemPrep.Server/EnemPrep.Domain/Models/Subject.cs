using System;
using System.Collections.Generic;
using EnemPrep.Domain.Enums;

namespace EnemPrep.Domain.Models;

public partial class Subject
{
    public Guid SubjectId { get; set; }
    
    public SubjectName Name { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<ExamSubject> ExamSubjects { get; set; } = new List<ExamSubject>();

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();

    public virtual ICollection<ScheduleSubject> ScheduleSubjects { get; set; } = new List<ScheduleSubject>();
}
