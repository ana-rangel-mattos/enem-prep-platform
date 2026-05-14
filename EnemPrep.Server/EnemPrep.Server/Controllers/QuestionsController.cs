using EnemPrep.Domain.Common;
using EnemPrep.Domain.Enums;
using EnemPrep.Server.Authorization;
using EnemPrep.ServicesContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnemPrep.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
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

        if (result is null)
            return NotFound($"Question with ID {id} could not be found.");

        return Ok(result);
    }
    
    [HttpGet]
    public async Task<IActionResult> FetchQuestions([FromQuery] QuestionQueryFilter filter, CancellationToken cancellationToken)
    {
        var result = await _questionService.FetchQuestionsAsync(filter, cancellationToken);

        return Ok(result);
    }
}