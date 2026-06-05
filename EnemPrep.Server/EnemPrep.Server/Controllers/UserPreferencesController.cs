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
    private IUserPreferencesService _userPreferencesService;
    
    public UserPreferencesController(IUserPreferencesService userPreferencesService)
    {
        _userPreferencesService = userPreferencesService;
    }

    [HttpPut("reset")]
    public async Task<IActionResult> Reset(CancellationToken cancellationToken)
    {
        var result = await _userPreferencesService.ResetUserPreferencesAsync(cancellationToken);

        return result.Match<IActionResult>(
            onSuccess: Ok,
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.UserPreferencesFailedToResetUserPreferences => StatusCode(StatusCodes.Status500InternalServerError, error.Description),
                    ErrorNames.UserPreferencesUserPreferenceWasNotFound => NotFound(error.Description),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error.Description)
                };
            });
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update(UpdateUserPreferencesDto request, CancellationToken cancellationToken)
    {
        var result = await _userPreferencesService.UpdateUserPreferencesAsync(request, cancellationToken);

        return result.Match<IActionResult>(
            onSuccess: Ok,
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.UserPreferencesFailedToUpdateUserPreferences => StatusCode(StatusCodes.Status500InternalServerError, error.Description),
                    ErrorNames.UserPreferencesUserPreferenceWasNotFound => NotFound(error.Description),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error.Description)
                };
            });
    }
}