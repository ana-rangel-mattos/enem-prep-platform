using EnemPrep.Domain.DTOS;
using EnemPrep.Domain.Result;

namespace EnemPrep.ServicesContracts;

public interface IUserService
{
    Task<Result> DeleteAccountAsync(Guid? userId, CancellationToken cancellationToken = default);
    Task<Result<GetUserDto>> FetchUserAsync(Guid? userId, CancellationToken cancellationToken = default);
}