using EnemPrep.Application.Features.Auth;
using EnemPrep.Domain.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnemPrep.Api.Controllers;

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
                    ErrorNames.LoginUserNotFound => BadRequest(error),
                    ErrorNames.LoginInvalidPassword => Unauthorized(error),
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
    public IActionResult Logout(CancellationToken cancellationToken = default)
    {
        var result = _authService.Logout();

        return result.Match<IActionResult>(
            onSuccess: Ok,
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.LogoutFailedToLogout => StatusCode(500, error),
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
                    ErrorNames.RegisterUserAlreadyExists => BadRequest(error),
                    ErrorNames.RegisterInvalidInvitationCode => StatusCode(StatusCodes.Status403Forbidden, error),
                    ErrorNames.RegisterFailedRegister => StatusCode(500, error),
                    _ => Problem(
                        title: "Internal Server Error",
                        detail: "Unexpected error occured while registering user.",
                        statusCode: StatusCodes.Status500InternalServerError
                    )
                };
            });
    }
}