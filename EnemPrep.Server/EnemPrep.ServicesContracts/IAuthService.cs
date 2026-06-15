using System.Text.Json;
using EnemPrep.Domain.DTOS;
using EnemPrep.Domain.Result;

namespace EnemPrep.ServicesContracts;

public interface IAuthService
{
    Task<Result> LoginAsync(LoginUserRequest request, CancellationToken cancellationToken = default);
    Result Logout(CancellationToken cancellationToken = default);
    Task<Result> RegisterAsync(PostUserRequest request, CancellationToken cancellationToken = default);
    Task<Result<GetUserDto>> FetchLoggedUserAsync(CancellationToken cancellationToken = default);
}
