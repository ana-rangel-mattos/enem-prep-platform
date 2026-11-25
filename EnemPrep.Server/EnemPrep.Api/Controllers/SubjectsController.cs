using EnemPrep.Application.Features.Subjects;
using EnemPrep.Domain.Enums;
using EnemPrep.Domain.Result;
using EnemPrep.Infrastructure.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnemPrep.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SubjectsController : ControllerBase
{
    private readonly ISubjectsService _subjectsService;
    
    public SubjectsController(ISubjectsService subjectsService)
    {
        _subjectsService = subjectsService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> FetchAll(CancellationToken cancellationToken)
    {
        ICollection<GetSubjectDto> subjects = await _subjectsService.FetchSubjectsAsync(cancellationToken);

        return Ok(subjects);
    }
    
    [HttpGet("{subjectId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> FetchSubjectById(Guid subjectId, CancellationToken cancellationToken)
    {
        var result = await _subjectsService.FetchSubjectByIdAsync(subjectId, cancellationToken);

        return result.Match<IActionResult>(
            onSuccess: () => Ok(result.Value),
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.SubjectNotFound => NotFound(error),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error)
                };
            });
    }

    [HasPermission(Permission.CreateSubject)]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateSubject([FromBody] PostSubjectDto request, CancellationToken cancellationToken)
    {
        var result = await _subjectsService.CreateSubjectAsync(request, cancellationToken);
        
        return result.Match<IActionResult>(
            onSuccess: Created,
            onFailure: error =>
            {
                return error.Code switch
                {
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error)
                };
            });
    }

    [HasPermission(Permission.DeleteSubject)]
    [HttpDelete("{subjectId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSubjectById(Guid subjectId, CancellationToken cancellationToken)
    {
        var result = await _subjectsService.DeleteSubjectByIdAsync(subjectId, cancellationToken);
        
        return result.Match<IActionResult>(
            onSuccess: Ok,
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.SubjectNotFound => NotFound(error),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error)
                };
            });
    }
}