using EnemPrep.Domain.DTOS;
using EnemPrep.Domain.Models;
using EnemPrep.ServicesContracts;
using EnemPrep.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EnemPrep.Services;

public class AuthService : IAuthService
{
    private readonly EnemContext _context;
    private readonly ISessionService _sessionService;
    
    public AuthService(EnemContext context, ISessionService sessionService)
    {
        _context = context;
        _sessionService = sessionService;
    }

    public async Task<AuthResponse> LoginAsync(LoginUserRequest request)
    {
        User? user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == request.Username);

        if (user is null)
        {
            return new AuthResponse(false, "User was not found.");
        }

        bool validPassword = BCrypt.Net.BCrypt.Verify(
            request.Password, 
            user.HashPassword);

        if (!validPassword)
        {
            return new AuthResponse(false, "Invalid password.");
        }
        
        _sessionService.SetUserSession(user.UserId, user.Username);

        return new AuthResponse(true, "Successfully logged in.");
    }
    
    public AuthResponse Logout()
    {
        _sessionService.RemoveUserSession();

        if (_sessionService.GetUserId() is not null)
        {
            return new AuthResponse(false, "Failed to logout.");
        }

        return new AuthResponse(true, "Successfully logged out.");
    }

    public async Task<AuthResponse> RegisterAsync(CreateUserRequest request)
    {
        User? existentUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username || u.Email == request.Email);

        if (existentUser is not null)
        {
            return new AuthResponse(false, "User already exists, try using another username or email.");
        }

        string? hashPassword = BCrypt.Net.BCrypt.HashPassword(request.Password, 12);

        if (hashPassword is null)
        {
            return GeneralFailedRegisterResponse();
        }

        User newUser = request.ConvertToUser();
        newUser.HashPassword = hashPassword;
        // newUser.UserRole = UserRole.Admin;
        
        await _context.Users.AddAsync(newUser);
        await _context.SaveChangesAsync();

        User? newlyCreatedUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);

        if (newlyCreatedUser is null)
        {
            return GeneralFailedRegisterResponse();
        }

        return new AuthResponse(true, "User was successfully registered.");
    }

    private AuthResponse GeneralFailedRegisterResponse()
    {
        return new AuthResponse(false, "Failed to register user, please try again.");
    }
}