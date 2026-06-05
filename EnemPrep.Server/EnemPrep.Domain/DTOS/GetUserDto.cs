namespace EnemPrep.Domain.DTOS;

public class GetUserDto
{
    public Guid UserId { get; set; }
    public string FullName { get; set; }
    public string Username { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Email { get; set; }
    public List<string> Roles = new List<string>();
}