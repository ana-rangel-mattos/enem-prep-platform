namespace EnemPrep.Domain.DTOS;

public class GetUserAccountDto
{
    public string FullName { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public bool IsPrivate { get; set; }
    public string? ImageUrl { get; set; }
}