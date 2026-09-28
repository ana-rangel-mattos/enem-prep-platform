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
[Authorize]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    
    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HasPermission(Permission.DeleteUser)]
    [HttpDelete("{userId:guid}")]
    public async Task<IActionResult> Delete(Guid userId, CancellationToken cancellationToken)
    {
        var result = await _userService.DeleteUserAccountAsync(userId, cancellationToken);

        return result.Match<IActionResult>(
            onSuccess: Ok,
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.UserNullUserId => BadRequest(error),
                    ErrorNames.UserUserNotFound => NotFound(error),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error)
                };
            }
        );
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetLoggedUser(CancellationToken cancellationToken)
    {
        var result = await _userService.FetchLoggedUserAsync(cancellationToken);
        
        return result.Match<IActionResult>(
            onSuccess: () => Ok(result.Value),
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.UserUserNotFound => NotFound(error),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error)
                };
            }
        );
    }

    [HttpGet("me/account")]
    public async Task<IActionResult> GetLoggedUserAccount(CancellationToken cancellationToken)
    {
        var result = await _userService.GetLoggedUserAccountAsync(cancellationToken);
        
        return result.Match<IActionResult>(
            onSuccess: () => Ok(result.Value),
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.UserUserNotFound => NotFound(error),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error)
                };
            }
        );
    }
    
    [HttpPut("me/account")]
    public async Task<IActionResult> UpdateLoggedUserAccount([FromBody] UpdateUserAccountDto request, CancellationToken cancellationToken)
    {
        var result = await _userService.UpdateLoggedUserAccountAsync(request, cancellationToken);
        
        return result.Match<IActionResult>(
            onSuccess: Ok,
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.UserUserNotFound => NotFound(error),
                    ErrorNames.UserInvalidPassword => BadRequest(error),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error)
                };
            }
        );
    }
    
    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetUserById(Guid userId, CancellationToken cancellationToken)
    {
        var result = await _userService.FetchUserByIdAsync(userId, cancellationToken);
        
        return result.Match<IActionResult>(
            onSuccess: () => Ok(result.Value),
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.UserNullUserId => BadRequest(error),
                    ErrorNames.UserUserNotFound => NotFound(error),
                    ErrorNames.UserPrivateUser => StatusCode(StatusCodes.Status403Forbidden, error),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error)
                };
            }
        );
    }
}