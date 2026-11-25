using EnemPrep.Application.Common.Models;
using EnemPrep.Domain.Result;

namespace EnemPrep.Application.Features.Articles;

/// <summary>
/// The article service interface.
/// </summary>
public interface IArticleService
{
    /// <summary>
    /// Creates a new Article with title, subtitle and Markdown blocks.
    /// </summary>
    /// <param name="request">The DTO containing the title, subtitle and Markdown blocks.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>A Result indicating success or failure with a specific article error.</returns>
    Task<Result> CreateArticleAsync(PostUpdateArticleDto request, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Updates the title, subtitles and Markdown blocks of an article object.
    /// </summary>
    /// <param name="articleId">The unique identifier of the target article.</param>
    /// <param name="request">The DTO object containing the title, subtitle and new Markdown blocks.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>A Result indicating success or failure with a specific article/playlist error.</returns>
    Task<Result> UpdateArticleAsync(Guid articleId, PostUpdateArticleDto request, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Fetches the newest articles available,
    /// does not contain the Markdown blocks.
    /// </summary>
    /// <param name="filter">The search params containing the page number, page size, sort by and search term.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>A Paged Response object containing the last articles created by the teachers.</returns>
    Task<PagedResponse<GetArticleDto>> FetchArticlesAsync(ArticleQueryFilter filter,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Fetches an article with Markdown blocks.
    /// </summary>
    /// <param name="articleId">The unique identifier of the target article.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>A Result with an article DTO if was successful, otherwise failure with a specific article error.</returns>
    Task<Result<GetArticleDetailDto>> FetchArticleByIdAsync(Guid articleId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Deletes an article object.
    /// </summary>
    /// <param name="articleId">The unique identifier of the target article.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>A Result indicating success or failure with a specific article error.</returns>
    Task<Result> DeleteArticleAsync(Guid articleId, CancellationToken cancellationToken = default);
}