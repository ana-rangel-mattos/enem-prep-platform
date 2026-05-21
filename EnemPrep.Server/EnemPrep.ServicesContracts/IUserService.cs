using EnemPrep.Domain.Result;

namespace EnemPrep.ServicesContracts;

public interface IUserService
{
    Task<Result> DeleteAccountAsync(Guid? userId, CancellationToken cancellationToken = default);
}