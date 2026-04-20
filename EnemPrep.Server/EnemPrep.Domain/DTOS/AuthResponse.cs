namespace EnemPrep.Domain.DTOS;

public class AuthResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }

    public AuthResponse(bool success, string? message)
    {
        Success = success;
        Message = message;
    }
}