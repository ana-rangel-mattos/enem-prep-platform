using EnemPrep.Domain.Common;
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
public class SolvedQuestionsController : ControllerBase
{
    private readonly ISolvedQuestionsService _solvedQuestionsService;

    public SolvedQuestionsController(ISolvedQuestionsService solvedQuestionsService)
    {
        _solvedQuestionsService = solvedQuestionsService;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> FetchSolvedById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _solvedQuestionsService.FetchSolvedQuestionByIdAsync(id, cancellationToken);

        return result.Match<IActionResult>(
            onSuccess: () => Ok(result.Value),
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.SolvedQuestionNotFound => NotFound(error.Description),
                    ErrorNames.SolvedQuestionNullId => BadRequest(error.Description),
                    ErrorNames.SolvedQuestionPrivateUser => StatusCode(StatusCodes.Status403Forbidden, error.Description),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error.Description)
                };
            }
        );
    }

    [HttpGet("user/{id:guid}")]
    public async Task<IActionResult> FetchSolvedByUserId(Guid id, [FromQuery] SolvedQuestionFilter filter, CancellationToken cancellationToken)
    {
        var result = await _solvedQuestionsService.FetchSolvedQuestionsByUserIdAsync(id, filter, cancellationToken);

        return result.Match<IActionResult>(
            onSuccess: () => Ok(result.Value),
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.SolvedQuestionNullUserId => BadRequest(error.Description),
                    ErrorNames.SolvedQuestionInexistentUser => NotFound(error.Description),
                    ErrorNames.SolvedQuestionPrivateUser => StatusCode(StatusCodes.Status403Forbidden, error.Description),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error.Description)
                };
            }
        );
    }

    [HttpPost("solve")]
    public async Task<IActionResult> SolveQuestion([FromBody] CreateSolvedQuestionDto request, CancellationToken cancellationToken)
    {
        var result = await _solvedQuestionsService.SolveQuestionAsync(request, cancellationToken);

        return result.Match<IActionResult>(
            onSuccess: Ok,
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.SolvedQuestionQuestionNotFound => NotFound(error.Description),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error.Description)
                };
            }
        );
    }
}