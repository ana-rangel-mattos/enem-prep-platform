using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EnemPrep.EntityModels.Enums;
using EnemPrep.EntityModels.Models;

namespace EnemPrep.EntityModels.DTOS;

public class CreateUserRequest
{
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

    public User ConvertToUser()
    {
        return new User()
        {
            Email = Email,
            Username = Username,
            DateOfBirth = DateOfBirth,
            FullName = FullName,
            IsPrivate = IsPrivate ?? false
        };
    }
}