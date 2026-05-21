using EnemPrep.Domain.DTOS;
using EnemPrep.Domain.Models;
using EnemPrep.Domain.Result;
using EnemPrep.Persistence.Constants;
using EnemPrep.ServicesContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnemPrep.Server.Controllers;

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
    public async Task<IActionResult> FetchAll(CancellationToken cancellationToken)
    {
        ICollection<SubjectDto> subjects = await _subjectsService.FetchSubjectsAsync(cancellationToken);

        return Ok(subjects);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> FetchSubjectById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _subjectsService.FetchSubjectByIdAsync(id, cancellationToken);

        return result.Match<IActionResult>(
            onSuccess: () => Ok(result.Value),
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.SubjectNotFound => NotFound(error.Description),
                    ErrorNames.SubjectNullId => BadRequest(error.Description),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error.Description)
                };
            });
    }
}