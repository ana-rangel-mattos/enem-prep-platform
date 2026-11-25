using System.ComponentModel.DataAnnotations;

namespace EnemPrep.Application.Features.Users;

public class GetUserDto
{
    public Guid UserId { get; set; }
    public string FullName { get; set; }
    public string Username { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Email { get; set; }
    public string? ImageUrl { get; set; } = string.Empty;
    public Int64 AttemptedQuestions { get; set; }
    public Int64 AttemptedExams { get; set; }
    public Int64 CorrectAnswers { get; set; }
    public float AverageScore { get; set; }
    public float HighestScore { get; set; }
    public float SuccessRate { get; set; }
    public TimeSpan AverageTimeSpent { get; set; }
    public bool HasGoal { get; set; }
    public List<string> Roles { get; set; } = new List<string>();
    public List<string> Permissions { get; set; } = new List<string>();
    public DateTime CreatedAt { get; set; }
}

public class GetUserAccountDto
{
    public string FullName { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public bool IsPrivate { get; set; }
    public string? ImageUrl { get; set; }
}

public class UpdateUserAccountDto
{
    public string? FullName { get; set; }
    
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string? Email { get; set; }
    
    [Length(8, 20, ErrorMessage = "Username must be between 8 and 20 characters.")]
    public string? Username { get; set; }
    public bool? IsPrivate { get; set; }
    public byte[]? ProfileImage { get; set; }
    public string? RoleInviteCode { get; set; }
    public string ConfirmationPassword { get; set; }
}