using EnemPrep.Domain.Result;

namespace EnemPrep.Application.Features.Auth;

public interface IAuthService
{
    Task<Result> LoginAsync(LoginUserRequest request, CancellationToken cancellationToken = default);
    Result Logout(CancellationToken cancellationToken = default);
    Task<Result> RegisterAsync(PostUserRequest request, CancellationToken cancellationToken = default);
}
