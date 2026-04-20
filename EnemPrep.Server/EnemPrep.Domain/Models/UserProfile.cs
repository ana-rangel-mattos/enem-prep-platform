using System;
using System.Collections.Generic;

namespace EnemPrep.Domain.Models;

public partial class UserProfile
{
    public Guid UserId { get; set; }

    public string? UserBio { get; set; }

    public int StreakCount { get; set; }

    public int ExperiencePoints { get; set; }

    public int CurrentLevel { get; set; }

    public DateTime LastActivityDate { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
