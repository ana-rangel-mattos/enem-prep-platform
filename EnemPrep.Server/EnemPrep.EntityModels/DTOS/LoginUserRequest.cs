using System.ComponentModel.DataAnnotations;

namespace EnemPrep.EntityModels.DTOS;

public class LoginUserRequest
{
    [Required(ErrorMessage = "Username is required.")]
    public string Username { get; set; }
    
    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; }
}