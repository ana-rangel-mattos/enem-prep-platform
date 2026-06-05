using EnemPrep.Domain.DTOS;
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
    
    public async Task<Result> DeleteAccountAsync(Guid? userId, CancellationToken cancellationToken = default)
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

    public async Task<Result<GetUserDto>> FetchUserAsync(Guid? userId, CancellationToken cancellationToken = default)
    {
        if (userId is null)
        {
            return Result.Failure<GetUserDto>(UserErrors.UserNullId);
        }
        
        Guid loggedUserId = _userContext.UserId;

        User? user = await _context.Users
            .Where(e => e.UserId == userId)
            .Include(e => e.Roles)
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
}