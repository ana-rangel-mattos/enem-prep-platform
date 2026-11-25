using EnemPrep.Domain.Result;

namespace EnemPrep.Application.Features.UserProfiles;

public interface IUserProfileService
{
    Task<Result<GetUserProfile>> FetchUserProfile(CancellationToken cancellationToken = default);
    Task<Result> UpdateUserBio(UpdateUserBio request, CancellationToken cancellationToken = default);
}