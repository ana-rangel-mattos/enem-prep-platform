using EnemPrep.Domain.DTOS;
using EnemPrep.Domain.Result;
using EnemPrep.Persistence.Constants;
using EnemPrep.Server.Authorization;
using EnemPrep.ServicesContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace EnemPrep.Server.Controllers;

[Route("/api/[controller]")]
public class AuthController : Controller
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
    public async Task<IActionResult> Register([FromBody] CreateUserRequest request)
    {
        var result = await _authService.RegisterAsync(request);

        return result.Match<IActionResult>(
            onSuccess: Ok,
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.RegisterUserAlreadyExists => BadRequest(error.Description),
                    ErrorNames.RegisterInvalidInvitationCode => Forbid(error.Description),
                    ErrorNames.RegisterFailedRegister => StatusCode(500, error.Description),
                    _ => Problem(
                        title: "Internal Server Error",
                        detail: "Unexpected error occured while registering user.",
                        statusCode: StatusCodes.Status500InternalServerError
                    )
                };
            });
    }
}