using EnemPrep.Application.Features.Questions;
using EnemPrep.Domain.Enums;
using EnemPrep.Domain.Result;
using EnemPrep.Infrastructure.Authorization;
using EnemPrep.ServicesContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnemPrep.Api.Controllers;

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
                    ErrorNames.QuestionNotFound => NotFound(error),
                    ErrorNames.QuestionNullId => BadRequest(error),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error)
                };
            }
        );
    }
    
    [HttpGet]
    public async Task<IActionResult> FetchQuestions([FromQuery] QuestionQueryFilter filter, CancellationToken cancellationToken)
    {
        var response = await _questionService.FetchQuestionsAsync(filter, cancellationToken);

        return Ok(response);
    }

    [HasPermission(Permission.CreateQuestions)]
    [HttpPost("new")]
    public async Task<IActionResult> Create([FromBody] PostQuestionDto request, CancellationToken cancellationToken)
    {
        var result = await _questionService.CreateQuestionAsync(request, cancellationToken);

        return result.Match<IActionResult>(
            onSuccess: Ok,
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.QuestionNullPublisherId => BadRequest(error),
                    ErrorNames.QuestionPublisherIsNotLoggedUser => StatusCode(StatusCodes.Status403Forbidden, error),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error)
                };
            }
        );
    }

    [HasPermission(Permission.DeleteQuestion)]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _questionService.RemoveQuestionAsync(id, cancellationToken);

        return result.Match<IActionResult>(
            onSuccess: Ok,
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.QuestionNotFound => NotFound(error),
                    ErrorNames.QuestionNullId => BadRequest(error),
                    ErrorNames.QuestionPublisherIsNotLoggedUser => StatusCode(StatusCodes.Status403Forbidden, error),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error)
                };
            });
    }

    [HttpGet("saved")]
    public async Task<IActionResult> FetchSavedQuestions([FromQuery] SavedQuestionFilter filter, CancellationToken cancellationToken)
    {
        var response = await _questionService.FetchSavedQuestions(filter, cancellationToken);

        return Ok(response);
    }

    [HttpPost("save")]
    public async Task<IActionResult> SaveQuestion([FromBody] PostSavedQuestionDto request, CancellationToken cancellationToken)
    {
        var result = await _questionService.SaveQuestionAsync(request, cancellationToken);

        return result.Match<IActionResult>(
            onSuccess: Ok,
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.SavedQuestionUserIsNotLoggedIn => Unauthorized(error),
                    ErrorNames.SavedQuestionQuestionNotFound => NotFound(error),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error)
                };
            });
    }
    
    [HttpDelete("{id:guid}/unsave")]
    public async Task<IActionResult> DeleteSavedQuestion(Guid id, CancellationToken cancellationToken)
    {
        var result = await _questionService.DeleteSavedQuestionAsync(id, cancellationToken);

        return result.Match<IActionResult>(
            onSuccess: Ok,
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.SavedQuestionUserIsNotLoggedIn => Unauthorized(error),
                    ErrorNames.SavedQuestionSavedQuestionNotFound => NotFound(error),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error)
                };
            });
    }
}