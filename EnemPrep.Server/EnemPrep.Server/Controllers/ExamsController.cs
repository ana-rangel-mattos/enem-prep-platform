using EnemPrep.Domain.Common;
using EnemPrep.Domain.DTOS;
using EnemPrep.Domain.Enums;
using EnemPrep.Domain.Result;
using EnemPrep.Persistence.Constants;
using EnemPrep.ServicesContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnemPrep.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ExamsController : ControllerBase
{
    private readonly IExamsService _examsService;
    
    public ExamsController(IExamsService examsService)
    {
        _examsService = examsService;
    }

    [HttpPost("new")]
    public async Task<IActionResult> GenerateExam(PostExamDto request, CancellationToken cancellationToken)
    {
        var result = await _examsService.GenerateExamAsync(request, cancellationToken);

        return result.Match<IActionResult>(
            onSuccess: () => StatusCode(StatusCodes.Status201Created, result.Value),
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.ExamInvalidSubjects => BadRequest(error.Description),
                    ErrorNames.ExamNoEnoughQuestionsForSubject => BadRequest(error.Description),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error.Description)
                };
            });
    }

    [HttpPut("{id:guid}/update")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromQuery] ExamStatus newStatus, CancellationToken cancellationToken)
    {
        UpdateExamStatusDto request = new UpdateExamStatusDto
        {
            ExamId = id,
            NewStatus = newStatus
        };
        
        var result = await _examsService.UpdateExamStatus(request, cancellationToken);
        
        return result.Match<IActionResult>(
            onSuccess: Ok,
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.ExamExamWasNotFound => NotFound(error.Description),
                    ErrorNames.ExamInvalidNewStatus => BadRequest(error.Description),
                    ErrorNames.ExamFailedToUpdateExamStatus => NotFound(error.Description),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error.Description)
                };
            });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> TakeExam(Guid id, CancellationToken cancellationToken)
    {
        var result = await _examsService.TakeExamAsync(id, cancellationToken);
        
        return result.Match<IActionResult>(
            onSuccess: () => Ok(result.Value),
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.ExamExamWasNotFound => NotFound(error.Description),
                    ErrorNames.ExamExamDoesNotBelongToLoggedUser => StatusCode(StatusCodes.Status403Forbidden, error.Description),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error.Description)
                };
            });
    }

    [HttpPost("{id:guid}/submit-answer")]
    public async Task<IActionResult> SubmitAnswer(Guid id, [FromBody] SubmitAnswerRequest request, CancellationToken cancellationToken)
    {
        request.ExamId = id;

        var result = await _examsService.SubmitAnswerAsync(request, cancellationToken);
        
        return result.Match<IActionResult>(
            onSuccess: Ok,
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.ExamExamQuestionWasNotFound => NotFound(error.Description),
                    ErrorNames.ExamFailedToSubmitQuestionAnswer => NotFound(error.Description),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error.Description)
                };
            });
    }

    [HttpPost("{id:guid}/complete")]
    public async Task<IActionResult> SubmitExam(Guid id, CancellationToken cancellationToken)
    {
        var result = await _examsService.SubmitExam(id, cancellationToken);
        
        return result.Match<IActionResult>(
            onSuccess: () => Ok(result.Value),
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.ExamExamWasNotFound => NotFound(error.Description),
                    ErrorNames.ExamExamDoesNotBelongToLoggedUser => StatusCode(StatusCodes.Status403Forbidden, error.Description),
                    ErrorNames.ExamFailedToCompleteExam => NotFound(error.Description),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error.Description)
                };
            });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] ExamQueryFilter filter, CancellationToken cancellationToken)
    {
        PagedResponse<GetExamDto> response = await _examsService.FetchAllExams(filter, cancellationToken);

        return Ok(response);
    }
}