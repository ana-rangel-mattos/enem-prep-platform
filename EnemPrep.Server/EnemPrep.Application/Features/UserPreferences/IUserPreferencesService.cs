using EnemPrep.Domain.Result;

namespace EnemPrep.Application.Features.UserPreferences;

public interface IUserPreferencesService
{
    Task<Result> ResetUserPreferencesAsync(CancellationToken cancellationToken = default);
    Task<Result> UpdateUserPreferencesAsync(UpdateUserPreferencesDto request, CancellationToken cancellationToken = default);
    Task<GetUserPreferencesDto> FetchLoggedUserPreferencesAsync(CancellationToken cancellationToken = default);
}