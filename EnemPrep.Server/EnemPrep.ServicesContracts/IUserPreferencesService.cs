using EnemPrep.Domain.DTOS;
using EnemPrep.Domain.Result;

namespace EnemPrep.ServicesContracts;

public interface IUserPreferencesService
{
    Task<Result> ResetUserPreferencesAsync(CancellationToken cancellationToken = default);
    Task<Result> UpdateUserPreferencesAsync(UpdateUserPreferencesDto request, CancellationToken cancellationToken = default);
}