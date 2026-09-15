using EnemPrep.Domain.DTOS;
using EnemPrep.Domain.Result;
using EnemPrep.Persistence.Constants;
using EnemPrep.ServicesContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnemPrep.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserPreferencesController : ControllerBase
{
    private readonly IUserPreferencesService _userPreferencesService;
    
    public UserPreferencesController(IUserPreferencesService userPreferencesService)
    {
        _userPreferencesService = userPreferencesService;
    }

    [HttpGet]
    public async Task<IActionResult> GetLoggedUserPreferences(CancellationToken cancellationToken)
    {
        var result = await _userPreferencesService.FetchLoggedUserPreferencesAsync(cancellationToken);

        return Ok(result);
    }

    [HttpPut("reset")]
    public async Task<IActionResult> ResetLoggedUserPreferences(CancellationToken cancellationToken)
    {
        var result = await _userPreferencesService.ResetUserPreferencesAsync(cancellationToken);

        return result.Match<IActionResult>(
            onSuccess: Ok,
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.UserPreferencesFailedToResetUserPreferences => StatusCode(StatusCodes.Status500InternalServerError, error),
                    ErrorNames.UserPreferencesUserPreferenceWasNotFound => NotFound(error),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error)
                };
            });
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateLoggedUserPreferences(UpdateUserPreferencesDto request, CancellationToken cancellationToken)
    {
        var result = await _userPreferencesService.UpdateUserPreferencesAsync(request, cancellationToken);

        return result.Match<IActionResult>(
            onSuccess: Ok,
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.UserPreferencesFailedToUpdateUserPreferences => StatusCode(StatusCodes.Status500InternalServerError, error),
                    ErrorNames.UserPreferencesUserPreferenceWasNotFound => NotFound(error),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error)
                };
            });
    }
}