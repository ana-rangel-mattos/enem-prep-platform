using System.Text.Json;
using EnemPrep.EntityModels.DTOS;

namespace EnemPrep.ServicesContracts;

public interface IAuthService
{
    Task<AuthResponse> LoginAsync(LoginUserRequest request);
    AuthResponse Logout();
    Task<AuthResponse> RegisterAsync(CreateUserRequest request);
}