using System.ComponentModel.DataAnnotations;
using EnemPrep.Domain.Models;

namespace EnemPrep.Application.Features.Auth;

public class LoginUserRequest
{
    [Required(ErrorMessage = "Username is required.")]
    public string Username { get; set; }
    
    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; }
}

public class PostUserRequest
{
    [MaxLength(50, ErrorMessage = "Invitation Code must be less than 51 characters.")]
    public string? Code { get; set; }
    
    [Required(ErrorMessage = "Full name is required")]
    public string FullName { get; set; }
    
    [Required(ErrorMessage = "Username is required")]
    [Length(8, 20, ErrorMessage = "Username must be between 8 and 20 characters.")]
    public string Username { get; set; }
    
    public DateTime DateOfBirth { get; set; }
    
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }

    public bool? IsPrivate { get; set; } = false;
    
    public byte[]? ProfileImage { get; set; }

    public User ConvertToUser()
    {
        return new User()
        {
            Email = Email,
            Username = Username,
            DateOfBirth = DateTime.SpecifyKind(
                DateOfBirth,
                DateTimeKind.Unspecified
            ),
            FullName = FullName,
            IsPrivate = IsPrivate ?? false,
            ProfileImage = ProfileImage
        };
    }
}