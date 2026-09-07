using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using EnemPrep.Domain.DTOS;
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

    public virtual ICollection<InvitationCode> CreatedInvitations { get; set; } = new List<InvitationCode>();

    public virtual ICollection<SavedQuestion> SavedQuestions { get; set; } = new List<SavedQuestion>();

    public GetUserDto ToGetUserDto()
    {
        string base64Image = Convert.ToBase64String(ProfileImage ?? Array.Empty<byte>());
        string imageLink = $"data:image/png;base64,{base64Image}";
        var roles = Roles.Select(r => r.Name).Distinct().ToList();
        var permissions = Roles.SelectMany(r => r.Permissions).Select(p => p.Name).Distinct().ToList();

        var solvedQuestionsCount = SolvedQuestions.Count;
        var correctAnswersCount = SolvedQuestions.Count(e => e.IsCorrect);

        bool hasExams = Exams.Any();
        bool hasSolvedQuestions = SolvedQuestions.Any();
        bool hasGoal = UserGoal is not null;

        return new GetUserDto
        {
            UserId = UserId,
            FullName = FullName,
            Username = Username,
            Email = Email,
            DateOfBirth = DateOfBirth,
            ImageUrl = imageLink,
            Roles = roles,
            AttemptedQuestions = solvedQuestionsCount,
            AttemptedExams = Exams.Count,
            CorrectAnswers = correctAnswersCount,
            AverageScore = hasExams ? Exams.Average(e => e.EstimatedScore ?? 0) : 0,
            HighestScore = hasExams ? Exams.Max(e => e.EstimatedScore ?? 0) : 0,
            SuccessRate = hasSolvedQuestions ? (float) correctAnswersCount / solvedQuestionsCount * 100 : 0,
            AverageTimeSpent = hasSolvedQuestions ? TimeSpan.FromTicks(Convert.ToInt64(SolvedQuestions.Average(s => s.TimeSpent.Ticks))) : TimeSpan.Zero,
            HasGoal = hasGoal,
            Permissions = permissions,
            CreatedAt = CreatedAt
        };
    }
}
