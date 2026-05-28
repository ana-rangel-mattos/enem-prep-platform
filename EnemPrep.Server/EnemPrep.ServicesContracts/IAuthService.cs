using System.Text.Json;
using EnemPrep.Domain.DTOS;
using EnemPrep.Domain.Result;

namespace EnemPrep.ServicesContracts;

public interface IAuthService
{
    Task<Result> LoginAsync(LoginUserRequest request);
    Result Logout();
    Task<Result> RegisterAsync(PostUserRequest request);
}
