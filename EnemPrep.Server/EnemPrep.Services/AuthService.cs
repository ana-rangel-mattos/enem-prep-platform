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
        if (await UserExists(request.Username, request.Email))
        {
            return new AuthResponse(false, "User already exists, try using another username or email.");
        }
        
        Role targetRole = Role.Student;
        if (!string.IsNullOrEmpty(request.Code))
        {
            InvitationCode? invite = await _context.InvitationCodes
                .Include(invitationCode => invitationCode.InviteRole)
                .FirstOrDefaultAsync(c => c.Code == request.Code && !c.IsUsed && c.ExpiresAt > DateTime.UtcNow);

            if (invite is null)
                return new AuthResponse(false, "Invalid invitation code.");

            targetRole = invite.InviteRole;
            invite.IsUsed = true;
        }

        string? hashPassword = BCrypt.Net.BCrypt.HashPassword(request.Password, 12);

        if (hashPassword is null)
        {
            return GeneralFailedRegisterResponse();
        }

        User newUser = request.ConvertToUser();
        newUser.HashPassword = hashPassword;
        newUser.Roles.Add(targetRole);
        
        await _context.Users.AddAsync(newUser);
        await _context.SaveChangesAsync();

        return new AuthResponse(true, "User was successfully registered.");
    }

    private async Task<bool> UserExists(string username, string email)
    {
        User? existentUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == username || u.Email == email);

        return existentUser is not null;
    }

    private AuthResponse GeneralFailedRegisterResponse()
    {
        return new AuthResponse(false, "Failed to register user, please try again.");
    }
}