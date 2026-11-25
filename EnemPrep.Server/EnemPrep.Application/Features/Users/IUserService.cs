using EnemPrep.Domain.Result;

namespace EnemPrep.Application.Features.Users;

public interface IUserService
{
    Task<Result> DeleteUserAccountAsync(Guid? userId, CancellationToken cancellationToken = default);
    Task<Result<GetUserDto>> FetchUserByIdAsync(Guid? userId, CancellationToken cancellationToken = default);
    Task<Result<GetUserDto>> FetchLoggedUserAsync(CancellationToken cancellationToken = default);
    Task<Result<GetUserAccountDto>> GetLoggedUserAccountAsync(CancellationToken cancellationToken = default);
    Task<Result> UpdateLoggedUserAccountAsync(UpdateUserAccountDto request, CancellationToken cancellationToken = default);
}