using EnemPrep.Domain.Common;
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
[Route("api/[controller]")]
[Authorize]
public class QuestionsController : ControllerBase
{
    private readonly IQuestionService _questionService;

    public QuestionsController(IQuestionService questionService)
    {
        _questionService = questionService;
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> FetchQuestionById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _questionService.FetchQuestionByIdAsync(id, cancellationToken);

        return result.Match<IActionResult>(
            onSuccess: () => Ok(result.Value),
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.QuestionNotFound => NotFound(error.Description),
                    ErrorNames.QuestionNullId => BadRequest(error.Description),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error.Description)
                };
            }
        );
    }
    
    [HttpGet]
    public async Task<IActionResult> FetchQuestions([FromQuery] QuestionQueryFilter filter, CancellationToken cancellationToken)
    {
        var result = await _questionService.FetchQuestionsAsync(filter, cancellationToken);

        return Ok(result);
    }

    [HasPermission(Permission.CreateQuestions)]
    [HttpPost("/new")]
    public async Task<IActionResult> Create([FromBody] CreateQuestionDto request, CancellationToken cancellationToken)
    {
        var result = await _questionService.CreateQuestionAsync(request, cancellationToken);

        return result.Match<IActionResult>(
            onSuccess: Ok,
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.QuestionNullPublisherId => BadRequest(error.Description),
                    ErrorNames.QuestionPublisherIsNotLoggedUser => StatusCode(StatusCodes.Status403Forbidden, error.Description),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error.Description)
                };
            }
        );
    }

    [HasPermission(Permission.DeleteQuestion)]
    [HttpDelete("/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _questionService.RemoveQuestionAsync(id, cancellationToken);

        return result.Match<IActionResult>(
            onSuccess: Ok,
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.QuestionNotFound => NotFound(error.Description),
                    ErrorNames.QuestionNullId => BadRequest(error.Description),
                    ErrorNames.QuestionPublisherIsNotLoggedUser => StatusCode(StatusCodes.Status403Forbidden, error.Description),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error.Description)
                };
            });
    }
}