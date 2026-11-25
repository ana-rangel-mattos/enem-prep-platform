using EnemPrep.Application.Features.UserGoals;
using EnemPrep.Domain.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnemPrep.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class UserGoalController : ControllerBase
{
    private readonly IUserGoalService _userGoalService;
    
    public UserGoalController(IUserGoalService userGoalService)
    {
        _userGoalService = userGoalService;
    }
    
    [HttpGet]
    public async Task<IActionResult> Fetch(CancellationToken cancellationToken)
    {
        var result = await _userGoalService.FetchUserGoalAsync(cancellationToken);

        return result.Match<IActionResult>(
            onSuccess: () => Ok(result.Value),
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.UserGoalNullUserId => Unauthorized(error),
                    ErrorNames.UserGoalUserNotFound => NotFound(error),
                    ErrorNames.UserGoalNotFound => NotFound(error),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error)
                };
            });
    }

    [HttpPost("new")]
    public async Task<IActionResult> Create([FromBody] PostUserGoalDto request,
        CancellationToken cancellationToken)
    {
        var result = await _userGoalService.CreateUserGoalAsync(request, cancellationToken);
        
        return result.Match<IActionResult>(
            onSuccess: Ok,
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.UserGoalNullUserId => Unauthorized(error),
                    ErrorNames.UserGoalUserNotFound => NotFound(error),
                    ErrorNames.UserGoalAlreadyExists => NotFound(error),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error)
                };
            });
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateUserGoalDto request, CancellationToken cancellationToken)
    {
        var result = await _userGoalService.UpdateUserGoalAsync(request, cancellationToken);

        return result.Match<IActionResult>(
            onSuccess: Ok,
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.UserGoalNullUserId => Unauthorized(error),
                    ErrorNames.UserGoalUserNotFound => NotFound(error),
                    ErrorNames.UserGoalNotFound => NotFound(error),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error)
                };
            });
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(CancellationToken cancellationToken)
    {
        var result = await _userGoalService.RemoveUserGoalAsync(cancellationToken);

        return result.Match<IActionResult>(
            onSuccess: Ok,
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.UserGoalNullUserId => Unauthorized(error),
                    ErrorNames.UserGoalUserNotFound => NotFound(error),
                    ErrorNames.UserGoalNotFound => NotFound(error),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error)
                };
            });
    }
}