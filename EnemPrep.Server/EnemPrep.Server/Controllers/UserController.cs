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
        var result = await _userService.DeleteAccountAsync(userId, cancellationToken);

        return result.Match<IActionResult>(
            onSuccess: Ok,
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.UserNullUserId => BadRequest(error.Description),
                    ErrorNames.UserUserNotFound => NotFound(error.Description),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error.Description)
                };
            }
        );
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] Guid userId, CancellationToken cancellationToken)
    {
        var result = await _userService.FetchUserAsync(userId, cancellationToken);
        
        return result.Match<IActionResult>(
            onSuccess: Ok,
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.UserNullUserId => BadRequest(error.Description),
                    ErrorNames.UserUserNotFound => NotFound(error.Description),
                    ErrorNames.UserPrivateUser => StatusCode(StatusCodes.Status403Forbidden, error.Description),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error.Description)
                };
            }
        );
    }
}