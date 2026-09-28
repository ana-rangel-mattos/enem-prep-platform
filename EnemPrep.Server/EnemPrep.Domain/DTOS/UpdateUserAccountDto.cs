using System.ComponentModel.DataAnnotations;

namespace EnemPrep.Domain.DTOS;

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