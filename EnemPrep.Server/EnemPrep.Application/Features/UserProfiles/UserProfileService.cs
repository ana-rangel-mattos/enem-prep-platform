using EnemPrep.Application.Common.Interfaces;
using EnemPrep.Domain.Result;
using EnemPrep.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EnemPrep.Application.Features.UserProfiles;

public class UserProfileService : IUserProfileService
{
    private readonly EnemContext _context;
    private readonly IUserContext _userContext;

    public UserProfileService(EnemContext context, IUserContext userContext)
    {
        _context = context;
        _userContext = userContext;
    }
    
    public async Task<Result<GetUserProfile>> FetchUserProfile(CancellationToken cancellationToken = default)
    {
        Guid userId = _userContext.UserId;

        var profile = await _context.UserProfiles
            .AsNoTracking()
            .SingleOrDefaultAsync(p => p.UserId == userId, cancellationToken);

        if (profile is null)
        {
            return Result.Failure<GetUserProfile>(UserProfileErrors.UserPreferenceWasNotFound);
        }

        return Result.Success(profile.ToGetUserProfile());
    }

    public async Task<Result> UpdateUserBio(UpdateUserBio request, CancellationToken cancellationToken = default)
    {
        Guid userId = _userContext.UserId;

        var profile = await _context.UserProfiles
            .SingleOrDefaultAsync(p => p.UserId == userId, cancellationToken);

        if (profile is null)
        {
            return Result.Failure<GetUserProfile>(UserProfileErrors.UserPreferenceWasNotFound);
        }

        profile.UserBio = request.UserBio;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}