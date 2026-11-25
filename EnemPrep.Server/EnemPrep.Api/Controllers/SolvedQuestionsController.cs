using EnemPrep.Application.Features.SolvedQuestions;
using EnemPrep.Domain.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnemPrep.Api.Controllers;

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

    [HttpGet]
    public async Task<IActionResult> FetchSolvedQuestions([FromQuery] SolvedQuestionFilter filter,
        CancellationToken cancellationToken)
    {
        var response = await _solvedQuestionsService.FetchUserSolvedQuestionsAsync(filter, cancellationToken);

        return Ok(response);
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
                    ErrorNames.SolvedQuestionNotFound => NotFound(error),
                    ErrorNames.SolvedQuestionNullId => BadRequest(error),
                    ErrorNames.SolvedQuestionPrivateUser => StatusCode(StatusCodes.Status403Forbidden, error),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error)
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
                    ErrorNames.SolvedQuestionNullUserId => BadRequest(error),
                    ErrorNames.SolvedQuestionInexistentUser => NotFound(error),
                    ErrorNames.SolvedQuestionPrivateUser => StatusCode(StatusCodes.Status403Forbidden, error),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error)
                };
            }
        );
    }

    [HttpPost("solve")]
    public async Task<IActionResult> SolveQuestion([FromBody] PostSolvedQuestionDto request, CancellationToken cancellationToken)
    {
        var result = await _solvedQuestionsService.SolveQuestionAsync(request, cancellationToken);

        return result.Match<IActionResult>(
            onSuccess: () => Ok(result.Value),
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.SolvedQuestionQuestionNotFound => NotFound(error),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error)
                };
            }
        );
    }
}