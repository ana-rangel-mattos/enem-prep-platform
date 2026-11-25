using EnemPrep.Application.Features.Articles;
using EnemPrep.Domain.Enums;
using EnemPrep.Domain.Result;
using EnemPrep.Infrastructure.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnemPrep.Api.Controllers;

/// <summary>
/// The controller for the article objects.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ArticlesController : ControllerBase
{
    private readonly IArticleService _articleService;

    /// <summary>
    /// Articles Controller Dependency Injection.
    /// </summary>
    /// <param name="articleService">The Articles service.</param>
    public ArticlesController(IArticleService articleService)
    {
        _articleService = articleService;
    }

    /// <inheritdoc cref="IArticleService.FetchArticlesAsync"/>
    [HasPermission(Permission.ReadArticle)]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetArticlesAsync([FromQuery] ArticleQueryFilter filter, CancellationToken cancellationToken)
    {
        var result = await _articleService.FetchArticlesAsync(filter, cancellationToken);

        return Ok(result);
    }

    /// <inheritdoc cref="IArticleService.FetchArticleByIdAsync"/>
    [HasPermission(Permission.ReadArticle)]
    [HttpGet("{articleId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetArticleByIdAsync(Guid articleId, CancellationToken cancellationToken)
    {
        var result = await _articleService.FetchArticleByIdAsync(articleId, cancellationToken);

        return result.Match<IActionResult>(
            onSuccess: () => Ok(result.Value),
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.ArticleArticleWasNotFound => NotFound(error),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error)
                };
            });
    }

    /// <inheritdoc cref="IArticleService.CreateArticleAsync"/>
    [HasPermission(Permission.CreateArticle)]
    [HttpPost("new")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateArticleAsync(PostUpdateArticleDto request, CancellationToken cancellationToken)
    {
        var result = await _articleService.CreateArticleAsync(request, cancellationToken);
        
        return result.Match<IActionResult>(
            onSuccess: Created,
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.ArticleInvalidPlaylistReference => BadRequest(error),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error)
                };
            });
    }

    /// <inheritdoc cref="IArticleService.UpdateArticleAsync"/>
    [HasPermission(Permission.EditArticle)]
    [HttpPut("{articleId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateArticleAsync(Guid articleId, PostUpdateArticleDto request,
        CancellationToken cancellationToken)
    {
        var result = await _articleService.UpdateArticleAsync(articleId, request, cancellationToken);
        
        return result.Match<IActionResult>(
            onSuccess: Ok,
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.ArticleInvalidPlaylistReference => BadRequest(error),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error)
                };
            });
    }

    /// <inheritdoc cref="IArticleService.DeleteArticleAsync"/>
    [HasPermission(Permission.DeleteArticle)]
    [HttpDelete("{articleId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteArticleByIdAsync(Guid articleId, CancellationToken cancellationToken)
    {
        var result = await _articleService.DeleteArticleAsync(articleId, cancellationToken);
        
        return result.Match<IActionResult>(
            onSuccess: Ok,
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.ArticleArticleWasNotFound => NotFound(error),
                    ErrorNames.ArticleCannotDeleteForbiddenAccess => StatusCode(StatusCodes.Status403Forbidden, error),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error)
                };
            });
    }
}