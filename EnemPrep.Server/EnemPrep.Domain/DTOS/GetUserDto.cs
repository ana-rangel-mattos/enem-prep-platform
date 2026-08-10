namespace EnemPrep.Domain.DTOS;

public class GetUserDto
{
    public Guid UserId { get; set; }
    public string FullName { get; set; }
    public string Username { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Email { get; set; }
    public string? ImageUrl { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new List<string>();
    public List<string> Permissions { get; set; } = new List<string>();
    public DateTime CreatedAt { get; set; }
}