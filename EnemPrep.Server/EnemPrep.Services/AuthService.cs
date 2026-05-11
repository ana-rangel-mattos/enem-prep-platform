using EnemPrep.Domain.DTOS;
using EnemPrep.Domain.Models;
using EnemPrep.ServicesContracts;
using EnemPrep.Persistence;
using Microsoft.EntityFrameworkCore;
using EnemPrep.Domain.Result;

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

    public async Task<Result> LoginAsync(LoginUserRequest request)
    {
        User? user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == request.Username);

        if (user is null)
        {
            return Result.Failure(AuthErrors.UserNotFound(request.Username));
        }

        bool validPassword = BCrypt.Net.BCrypt.Verify(
            request.Password,
            user.HashPassword);

        if (!validPassword)
        {
            return Result.Failure(AuthErrors.InvalidPassword);
        }

        _sessionService.SetUserSession(user.UserId, user.Username);

        return Result.Success();
    }

    public Result Logout()
    {
        _sessionService.RemoveUserSession();
        
        if (_sessionService.GetUserId() is not null)
        {
            return Result.Failure(AuthErrors.FailedToLogout);
        }

        return Result.Success();
    }

    public async Task<Result> RegisterAsync(CreateUserRequest request)
    {
        if (await UserExists(request.Username, request.Email))
        {
            return Result.Failure(AuthErrors.UserAlreadyExists);
        }

        Role targetRole = Role.Student;
        if (!string.IsNullOrEmpty(request.Code))
        {
            InvitationCode? invite = await _context.InvitationCodes
                .Include(invitationCode => invitationCode.InviteRole)
                .FirstOrDefaultAsync(c => c.Code == request.Code && !c.IsUsed && c.ExpiresAt > DateTime.UtcNow);

            if (invite is null)
                return Result.Failure(AuthErrors.InvalidInvitationCode);

            targetRole = invite.InviteRole;
            invite.IsUsed = true;
        }

        string? hashPassword = BCrypt.Net.BCrypt.HashPassword(request.Password, 12);

        if (hashPassword is null)
        {
            return Result.Failure(AuthErrors.FailedRegister);
        }

        User newUser = request.ConvertToUser();
        newUser.HashPassword = hashPassword;
        newUser.Roles.Add(targetRole);

        await _context.Users.AddAsync(newUser);
        await _context.SaveChangesAsync();

        return Result.Success();
    }

    private async Task<bool> UserExists(string username, string email)
    {
        User? existentUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == username || u.Email == email);

        return existentUser is not null;
    }
}
