using System;
using System.Collections.Generic;

namespace EnemPrep.Domain.Models;

public partial class UserGoal
{
    public Guid UserGoalId { get; set; }

    public Guid UserId { get; set; }

    public string UniversityName { get; set; } = null!;

    public string CourseName { get; set; } = null!;

    public float CutOffScore { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
