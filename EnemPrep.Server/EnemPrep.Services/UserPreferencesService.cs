using EnemPrep.Domain.DTOS;
using EnemPrep.Domain.Result;
using EnemPrep.ServicesContracts;

namespace EnemPrep.Services;

public class UserPreferencesService : IUserPreferencesService
{
    public Task<Result> ResetUserPreferencesAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result> UpdateUserPreferencesAsync(UpdateUserPreferencesDto request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}