using EnemPrep.Application.Features.Schedules;
using EnemPrep.Domain.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnemPrep.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SchedulesController : ControllerBase
{
    private readonly IScheduleService _scheduleService;

    public SchedulesController(IScheduleService scheduleService)
    {
        _scheduleService = scheduleService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllSchedules(CancellationToken cancellationToken)
    {
        var result = await _scheduleService.FetchAllSchedulesAsync(cancellationToken);

        return Ok(result);
    }

    [HttpGet("{scheduleId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetScheduleById(Guid scheduleId, CancellationToken cancellationToken)
    {
        var result = await _scheduleService.FetchScheduleByIdAsync(scheduleId, cancellationToken);
        
        return result.Match<IActionResult>(
            onSuccess: () => Ok(result.Value),
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.ScheduleScheduleWasNotFound => NotFound(error),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error)
                };
            });
    }

    [HttpPost("new")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> PostScheduleAsync([FromBody] PostScheduleDto request,
        CancellationToken cancellationToken)
    {
        var result = await _scheduleService.PostScheduleAsync(request, cancellationToken);
        
        return result.Match<IActionResult>(
            onSuccess: Ok,
            onFailure: error =>
            {
                return error.Code switch
                {
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error)
                };
            });
    }
    
    [HttpPost("{scheduleId:guid}/subjects/new")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PostScheduleSubjectAsync(Guid scheduleId, [FromBody] PostScheduleSubjectDto request,
        CancellationToken cancellationToken)
    {
        var result = await _scheduleService.PostScheduleSubjectAsync(scheduleId, request, cancellationToken);
        
        return result.Match<IActionResult>(
            onSuccess: Ok,
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.ScheduleScheduleWasNotFound => NotFound(error),
                    ErrorNames.ScheduleScheduleSubjectWasNotFound => NotFound(error),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error)
                };
            });
    }

    [HttpDelete("{scheduleId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteScheduleAsync(Guid scheduleId, CancellationToken cancellationToken)
    {
        var result = await _scheduleService.DeleteScheduleAsync(scheduleId, cancellationToken);
        
        return result.Match<IActionResult>(
            onSuccess: Ok,
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.ScheduleScheduleWasNotFound => NotFound(error),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error)
                };
            });
    }

    [HttpDelete("{scheduleId:guid}/subjects/{scheduleSubjectId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteScheduleSubjectAsync(Guid scheduleId, Guid scheduleSubjectId,
        CancellationToken cancellationToken)
    {
        var result = await _scheduleService.DeleteScheduleSubjectAsync(scheduleId, scheduleSubjectId, cancellationToken);
        
        return result.Match<IActionResult>(
            onSuccess: Ok,
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.ScheduleScheduleSubjectWasNotFound => NotFound(error),
                    ErrorNames.ScheduleSubjectDeleteAccessForbidden => StatusCode(StatusCodes.Status403Forbidden, error),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error)
                };
            });
    }
    
    [HttpPut("{scheduleId:guid}/reorder")]
    public async Task<IActionResult> UpdateScheduleOrderAsync(Guid scheduleId, [FromBody] UpdateScheduleSubjectOrderDto request, CancellationToken cancellationToken)
    {
        var result = await _scheduleService.UpdateScheduleSubjectOrderAsync(scheduleId, request, cancellationToken);
        
        return result.Match<IActionResult>(
            onSuccess: Ok,
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.ScheduleScheduleWasNotFound => NotFound(error),
                    ErrorNames.ScheduleScheduleSubjectWasNotFound => NotFound(error),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error)
                };
            });
    }

    [HttpPut("{scheduleId:guid}/update")]
    public async Task<IActionResult> UpdateScheduleById(Guid scheduleId, [FromBody] UpdateScheduleDto request,
        CancellationToken cancellationToken)
    {
        var result = await _scheduleService.UpdateScheduleByIdAsync(scheduleId, request, cancellationToken);
        
        return result.Match<IActionResult>(
            onSuccess: Ok,
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.ScheduleScheduleWasNotFound => NotFound(error),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error)
                };
            });
    }
}