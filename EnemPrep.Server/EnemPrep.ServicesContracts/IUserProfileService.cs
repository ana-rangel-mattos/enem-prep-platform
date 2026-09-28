using EnemPrep.Domain.DTOS;
using EnemPrep.Domain.Result;

namespace EnemPrep.ServicesContracts;

public interface IUserProfileService
{
    Task<Result<GetUserProfile>> FetchUserProfile(CancellationToken cancellationToken = default);
    Task<Result> UpdateUserBio(UpdateUserBio request, CancellationToken cancellationToken = default);
}