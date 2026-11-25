using EnemPrep.Domain.Models;

namespace EnemPrep.Application.Features.Users;

public static class UserExtensions
{
    public static GetUserDto ToGetUserDto(this User user)
    {
        var roles = user.Roles.Select(r => r.Name).Distinct().ToList();
        var permissions = user.Roles.SelectMany(r => r.Permissions).Select(p => p.Name).Distinct().ToList();

        var solvedQuestionsCount = user.SolvedQuestions.Count;
        var correctAnswersCount = user.SolvedQuestions.Count(e => e.IsCorrect);

        bool hasExams = user.Exams.Any();
        bool hasSolvedQuestions = user.SolvedQuestions.Any();
        bool hasGoal = user.UserGoal is not null;

        return new GetUserDto
        {
            UserId = user.UserId,
            FullName = user.FullName,
            Username = user.Username,
            Email = user.Email,
            DateOfBirth = user.DateOfBirth,
            ImageUrl = GetUserImageUrl(user),
            Roles = roles,
            AttemptedQuestions = solvedQuestionsCount,
            AttemptedExams = user.Exams.Count,
            CorrectAnswers = correctAnswersCount,
            AverageScore = hasExams ? user.Exams.Average(e => e.EstimatedScore ?? 0) : 0,
            HighestScore = hasExams ? user.Exams.Max(e => e.EstimatedScore ?? 0) : 0,
            SuccessRate = hasSolvedQuestions ? (float) correctAnswersCount / solvedQuestionsCount * 100 : 0,
            AverageTimeSpent = hasSolvedQuestions ? TimeSpan.FromTicks(Convert.ToInt64(user.SolvedQuestions.Average(s => s.TimeSpent.Ticks))) : TimeSpan.Zero,
            HasGoal = hasGoal,
            Permissions = permissions,
            CreatedAt = user.CreatedAt
        };
    }

    public static GetUserAccountDto ToGetUserAccountDto(this User user)
    {
        return new GetUserAccountDto
        {
            FullName = user.FullName,
            Email = user.Email,
            IsPrivate = user.IsPrivate,
            ImageUrl = GetUserImageUrl(user),
            Username = user.Username
        };
    }
    
    private static string GetUserImageUrl(User user)
    {
        string base64Image = Convert.ToBase64String(user.ProfileImage ?? Array.Empty<byte>());
        string imageLink = $"data:image/png;base64,{base64Image}";

        return imageLink;
    }
}