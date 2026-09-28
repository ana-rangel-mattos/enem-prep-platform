using EnemPrep.Domain.DTOS;
using EnemPrep.Domain.Extensions;
using EnemPrep.Domain.Models;
using EnemPrep.Domain.Result;
using EnemPrep.Persistence;
using EnemPrep.ServicesContracts;
using Microsoft.EntityFrameworkCore;

namespace EnemPrep.Services;

public class UserService : IUserService
{
    private readonly EnemContext _context;
    private readonly IUserContext _userContext;

    public UserService(EnemContext context, IUserContext userContext)
    {
        _context = context;
        _userContext = userContext;
    }
    
    public async Task<Result> DeleteUserAccountAsync(Guid? userId, CancellationToken cancellationToken = default)
    {
        if (userId is null)
        {
            return Result.Failure(UserErrors.UserNullId);
        }

        var deletedRows = await _context.Users
            .Where(u => u.UserId == userId)
            .ExecuteDeleteAsync(cancellationToken);

        if (deletedRows == 0)
        {
            return Result.Failure(UserErrors.UserNotFound(userId));
        }

        return Result.Success();
    }

    public async Task<Result<GetUserDto>> FetchUserByIdAsync(Guid? userId, CancellationToken cancellationToken = default)
    {
        if (userId is null)
        {
            return Result.Failure<GetUserDto>(UserErrors.UserNullId);
        }
        
        Guid loggedUserId = _userContext.UserId;

        User? user = await _context.Users
            .AsNoTracking()
            .AsSplitQuery()
            .Include(e => e.Roles)
            .ThenInclude(r => r.Permissions)
            .Include(e => e.SolvedQuestions)
            .Include(e => e.Exams)
            .Include(e => e.UserGoal)
            .Where(e => e.UserId == loggedUserId)
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            return Result.Failure<GetUserDto>(UserErrors.UserNotFound(userId));
        }
        
        if (user.UserId != loggedUserId && user.IsPrivate)
        {
            return Result.Failure<GetUserDto>(UserErrors.PrivateUser);
        }

        return Result.Success(user.ToGetUserDto());
    }

    public async Task<Result<GetUserDto>> FetchLoggedUserAsync(CancellationToken cancellationToken = default)
    {
        Guid loggedUserId = _userContext.UserId;

        User? user = await _context.Users
            .AsNoTracking()
            .AsSplitQuery()
            .Include(e => e.Roles)
            .ThenInclude(r => r.Permissions)
            .Include(e => e.SolvedQuestions)
            .Include(e => e.Exams)
            .Include(e => e.UserGoal)
            .Where(e => e.UserId == loggedUserId)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (user is null)
        {
            return Result.Failure<GetUserDto>(UserErrors.UserNotFound(loggedUserId));
        }

        return Result.Success(user.ToGetUserDto());
    }

    public async Task<Result<GetUserAccountDto>> GetLoggedUserAccountAsync(CancellationToken cancellationToken = default)
    {
        Guid loggedUserId = _userContext.UserId;

        var user = await _context.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.UserId == loggedUserId, cancellationToken);

        if (user is null)
        {
            return Result.Failure<GetUserAccountDto>(UserErrors.UserNotFound(loggedUserId));
        }

        return Result.Success(user.ToGetUserAccountDto());
    }

    public async Task<Result> UpdateLoggedUserAccountAsync(UpdateUserAccountDto request, CancellationToken cancellationToken = default)
    {
        Guid loggedUserId = _userContext.UserId;

        var user = await _context.Users
            .SingleOrDefaultAsync(u => u.UserId == loggedUserId, cancellationToken);
        
        if (user is null)
        {
            return Result.Failure<GetUserAccountDto>(UserErrors.UserNotFound(loggedUserId));
        }

        bool validPassword = BCrypt.Net.BCrypt.Verify(
            request.ConfirmationPassword, user.HashPassword);

        if (!validPassword)
        {
            return Result.Failure(UserErrors.InvalidPassword);
        }

        user.FullName = request.FullName ?? user.FullName;
        user.Email = request.Email ?? user.Email;
        user.Username = request.Username ?? user.Username;
        user.ProfileImage = request.ProfileImage ?? user.ProfileImage;
        user.IsPrivate = request.IsPrivate ?? user.IsPrivate;

        if (!string.IsNullOrEmpty(request.RoleInviteCode))
        {
            InvitationCode? invite = await _context.InvitationCodes
                .Include(ic => ic.InviteRole)
                .FirstOrDefaultAsync(c => c.Code == request.RoleInviteCode && !c.IsUsed && c.ExpiresAt > DateTime.UtcNow, cancellationToken);

            if (invite is null)
            {
                return Result.Failure(UserErrors.InvalidRoleInviteCode);
            }
            
            user.Roles.Add(invite.InviteRole);
            invite.IsUsed = true;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}