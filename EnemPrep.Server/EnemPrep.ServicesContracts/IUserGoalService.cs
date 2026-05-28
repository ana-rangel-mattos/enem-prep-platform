using EnemPrep.Domain.DTOS;
using EnemPrep.Domain.Result;

namespace EnemPrep.ServicesContracts;

public interface IUserGoalService
{
    Task<Result<GetUserGoalDto>> FetchUserGoalAsync(CancellationToken cancellationToken = default);
    Task<Result> CreateUserGoalAsync(PostUserGoalDto request, CancellationToken cancellationToken = default);
    Task<Result> UpdateUserGoalAsync(UpdateUserGoalDto request, CancellationToken cancellationToken = default);
    Task<Result> RemoveUserGoalAsync(CancellationToken cancellationToken = default);
}