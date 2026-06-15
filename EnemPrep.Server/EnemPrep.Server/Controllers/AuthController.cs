using EnemPrep.Domain.DTOS;
using EnemPrep.Domain.Enums;
using EnemPrep.Domain.Result;
using EnemPrep.Persistence.Constants;
using EnemPrep.Server.Authorization;
using EnemPrep.ServicesContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnemPrep.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    [HttpPost("[action]")]
    public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
    {
        var result = await _authService.LoginAsync(request);

        return result.Match<IActionResult>(
            onSuccess: Ok,
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.LoginUserNotFound => BadRequest(error.Description),
                    ErrorNames.LoginInvalidPassword => Unauthorized(error.Description),
                    _ => Problem(
                        title: "Internal Server Error",
                        detail: "Unexpected error occured while logging in user.",
                        statusCode: StatusCodes.Status500InternalServerError
                    )
                };
            });
    }
    
    [Authorize]
    [HttpPost("[action]")]
    public IActionResult Logout()
    {
        var result = _authService.Logout();

        return result.Match<IActionResult>(
            onSuccess: Ok,
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.LogoutFailedToLogout => StatusCode(500, error.Description),
                    _ => Problem(
                        title: "Internal Server Error",
                        detail: "Unexpected error occured while logging out user.",
                        statusCode: StatusCodes.Status500InternalServerError
                    )
                };
            });
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Register([FromBody] PostUserRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.RegisterAsync(request, cancellationToken);

        return result.Match<IActionResult>(
            onSuccess: Ok,
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.RegisterUserAlreadyExists => BadRequest(error.Description),
                    ErrorNames.RegisterInvalidInvitationCode => StatusCode(StatusCodes.Status403Forbidden, error.Description),
                    ErrorNames.RegisterFailedRegister => StatusCode(500, error.Description),
                    _ => Problem(
                        title: "Internal Server Error",
                        detail: "Unexpected error occured while registering user.",
                        statusCode: StatusCodes.Status500InternalServerError
                    )
                };
            });
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetUser(CancellationToken cancellationToken)
    {
        var result = await _authService.FetchLoggedUserAsync(cancellationToken);
        
        return result.Match<IActionResult>(
            onSuccess: () => Ok(result.Value),
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.FetchLoggedUserUserNotFound => NotFound(error.Description),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error.Description)
                };
            });
    }
}