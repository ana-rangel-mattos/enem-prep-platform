using EnemPrep.Domain.DTOS;
using EnemPrep.Domain.Enums;
using EnemPrep.Domain.Models;
using EnemPrep.ServicesContracts;
using EnemPrep.Persistence;
using Microsoft.EntityFrameworkCore;
using EnemPrep.Domain.Result;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace EnemPrep.Services;

public class AuthService : IAuthService
{
    private readonly EnemContext _context;
    private readonly ISessionService _sessionService;
    private readonly IUserContext _userContext;

    public AuthService(EnemContext context, ISessionService sessionService, IUserContext userContext)
    {
        _context = context;
        _sessionService = sessionService;
        _userContext = userContext;
    }

    public async Task<Result> LoginAsync(LoginUserRequest request, CancellationToken cancellationToken = default)
    {
        User? user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == request.Username, cancellationToken);

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

    public Result Logout(CancellationToken cancellationToken = default)
    {
        _sessionService.RemoveUserSession();
        
        if (_sessionService.GetUserId() is not null)
        {
            return Result.Failure(AuthErrors.FailedToLogout);
        }

        return Result.Success();
    }

    public async Task<Result> RegisterAsync(PostUserRequest request, CancellationToken cancellationToken = default)
    {
        if (await UserExists(request.Username, request.Email))
        {
            return Result.Failure(AuthErrors.UserAlreadyExists);
        }
        
        string? hashPassword = BCrypt.Net.BCrypt.HashPassword(request.Password, 12);

        if (hashPassword is null)
        {
            return Result.Failure(AuthErrors.FailedRegister);
        }
        
        User newUser = request.ConvertToUser();
        newUser.HashPassword = hashPassword;

        var studentRole = await _context.Roles
            .FirstOrDefaultAsync(r => r.Id == Role.Student.Id, cancellationToken);
        
        if (studentRole is not null)
        {
            newUser.Roles.Add(studentRole);
        }
        
        if (!string.IsNullOrEmpty(request.Code))
        {
            InvitationCode? invite = await _context.InvitationCodes
                .Include(invitationCode => invitationCode.InviteRole)
                .FirstOrDefaultAsync(c => c.Code == request.Code && !c.IsUsed && c.ExpiresAt > DateTime.UtcNow, cancellationToken);

            if (invite is null)
                return Result.Failure(AuthErrors.InvalidInvitationCode);
            
            newUser.Roles.Add(invite.InviteRole);
            invite.IsUsed = true;
        }
        
        UpdateUserPreferencesDto userPreferencesDto = new UpdateUserPreferencesDto
        {
            ColorScheme = ColorScheme.OS,
            ExamLanguage = Language.English,
            QuestionsPerDay = 5,
        };
        newUser.UserPreferences.Add(userPreferencesDto.ConvertToUserPreference());

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result<GetUserDto>> FetchLoggedUserAsync(CancellationToken cancellationToken = default)
    {
        Guid userId = _userContext.UserId;

        User? user = await _context.Users
            .AsNoTracking()
            .Include(u => u.Roles)
            .ThenInclude(r => r.Permissions)
            .FirstOrDefaultAsync(u => u.UserId == userId, cancellationToken);
        
        if (user is null)
        {
            return Result.Failure<GetUserDto>(AuthErrors.UserNotFoundId(userId));
        }

        return Result.Success(user.ToGetUserDto());
    }

    private async Task<bool> UserExists(string username, string email)
    {
        User? existentUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == username || u.Email == email);

        return existentUser is not null;
    }
}
