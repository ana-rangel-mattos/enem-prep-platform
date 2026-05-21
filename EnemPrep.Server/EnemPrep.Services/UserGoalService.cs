using EnemPrep.Domain.DTOS;
using EnemPrep.Domain.Models;
using EnemPrep.Domain.Result;
using EnemPrep.Persistence;
using EnemPrep.ServicesContracts;
using Microsoft.EntityFrameworkCore;

namespace EnemPrep.Services;

public class UserGoalService : IUserGoalService
{
    private readonly EnemContext _context;
    private readonly ISessionService _sessionService;
    
    public UserGoalService(EnemContext context, ISessionService sessionService)
    {
        _context = context;
        _sessionService = sessionService;
    }
    
    public async Task<Result<GetUserGoalDto>> FetchUserGoalAsync(CancellationToken cancellationToken = default)
    {
        Guid? userId = _sessionService.GetUserId();

        if (userId is null)
        {
            return Result.Failure<GetUserGoalDto>(UserGoalErrors.NullUserId);
        }

        var user = await _context.Users
            .Where(u => u.UserId == userId)
            .Include(u => u.UserGoal)
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            return Result.Failure<GetUserGoalDto>(UserGoalErrors.UserNotFound(userId));
        }

        if (user.UserGoal is null)
        {
            return Result.Failure<GetUserGoalDto>(UserGoalErrors.UserGoalNotFound);
        }

        return Result.Success(new GetUserGoalDto
        {
            UserId = user.UserId,
            UserGoalId = user.UserGoal.UserGoalId,
            CourseName = user.UserGoal.CourseName,
            CutOffScore = user.UserGoal.CutOffScore,
            UniversityName = user.UserGoal.UniversityName
        });
    }

    public async Task<Result> CreateUserGoalAsync(CreateUserGoalDto request, CancellationToken cancellationToken = default)
    {
        Guid? userId = _sessionService.GetUserId();

        if (userId is null)
        {
            return Result.Failure(UserGoalErrors.NullUserId);
        }

        var user = await _context.Users
            .Where(u => u.UserId == userId)
            .Include(u => u.UserGoal)
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            return Result.Failure(UserGoalErrors.UserNotFound(userId));
        }

        if (user.UserGoal is not null)
        {
            return Result.Failure(UserGoalErrors.UserGoalAlreadyExists);
        }

        UserGoal newUserGoal = request.ToUserGoal();
        newUserGoal.UserId = user.UserId;

        await _context.UserGoals.AddAsync(newUserGoal, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }

    public async Task<Result> UpdateUserGoalAsync(UpdateUserGoalDto request, CancellationToken cancellationToken = default)
    {
        Guid? userId = _sessionService.GetUserId();

        if (userId is null)
        {
            return Result.Failure(UserGoalErrors.NullUserId);
        }

        var user = await _context.Users
            .Where(u => u.UserId == userId)
            .Include(u => u.UserGoal)
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            return Result.Failure(UserGoalErrors.UserNotFound(userId));
        }

        if (user.UserGoal is null)
        {
            return Result.Failure(UserGoalErrors.UserGoalNotFound);
        }

        await _context.UserGoals
            .Where(g => g.UserId == user.UserId)
            .ExecuteUpdateAsync(e => e
                .SetProperty(x => x.CourseName, request.CourseName)
                .SetProperty(x => x.UniversityName, request.UniversityName)
                .SetProperty(x => x.CutOffScore, request.CutOffScore), cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }

    public async Task<Result> RemoveUserGoalAsync(CancellationToken cancellationToken = default)
    {
        Guid? userId = _sessionService.GetUserId();

        if (userId is null)
        {
            return Result.Failure(UserGoalErrors.NullUserId);
        }

        var user = await _context.Users
            .Where(u => u.UserId == userId)
            .Include(u => u.UserGoal)
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            return Result.Failure(UserGoalErrors.UserNotFound(userId));
        }

        int deletedRows = await _context.UserGoals
            .Where(u => u.UserId == user.UserId)
            .ExecuteDeleteAsync(cancellationToken);

        if (deletedRows == 0)
        {
            return Result.Failure(UserGoalErrors.UserGoalNotFound);
        }
        
        await _context.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}