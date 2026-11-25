using EnemPrep.Application.Features.UserProfiles;
using EnemPrep.Domain.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnemPrep.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserProfileController: ControllerBase
{
    private readonly IUserProfileService _userProfileService;

    public UserProfileController(IUserProfileService userProfileService)
    {
        _userProfileService = userProfileService;
    }

    [HttpGet]
    public async Task<IActionResult> GetProfile(CancellationToken cancellationToken)
    {
        var result = await _userProfileService.FetchUserProfile(cancellationToken);

        return result.Match<IActionResult>(
            onSuccess: () => Ok(result.Value),
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.UserProfileUserProfileWasNotFound => NotFound(error),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error)
                };
            });
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProfileBio([FromBody] UpdateUserBio request,
        CancellationToken cancellationToken)
    {
        var result = await _userProfileService.UpdateUserBio(request, cancellationToken);
        
        return result.Match<IActionResult>(
            onSuccess: Ok,
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.UserProfileUserProfileWasNotFound => NotFound(error),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error)
                };
            });
    }
}