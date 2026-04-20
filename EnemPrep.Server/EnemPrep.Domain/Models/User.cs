using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using EnemPrep.Domain.Enums;

namespace EnemPrep.Domain.Models;

public partial class User
{
    public Guid UserId { get; set; }

    public string FullName { get; set; } = null!;

    public string Username { get; set; } = null!;

    public DateTime DateOfBirth { get; set; }

    public string Email { get; set; } = null!;

    public string HashPassword { get; set; } = null!;

    public bool IsPrivate { get; set; }

    public byte[]? ProfileImage { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

    public virtual ICollection<SolvedQuestion> SolvedQuestions { get; set; } = new List<SolvedQuestion>();

    public virtual UserGoal? UserGoal { get; set; }

    public virtual ICollection<UserPreference> UserPreferences { get; set; } = new List<UserPreference>();

    public virtual UserProfile? UserProfile { get; set; }
}
