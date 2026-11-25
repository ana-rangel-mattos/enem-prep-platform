using System.Text.Json;
using EnemPrep.Application.Common.Interfaces;
using EnemPrep.Application.Common.Models;
using EnemPrep.Application.Common.Querying;
using EnemPrep.Domain.Common;
using EnemPrep.Domain.Models;
using EnemPrep.Domain.Models.Blocks;
using EnemPrep.Domain.Result;
using EnemPrep.Persistence;
using EnemPrep.ServicesContracts;
using Microsoft.EntityFrameworkCore;

namespace EnemPrep.Application.Features.Articles;

public class ArticleService : IArticleService
{
    private readonly EnemContext _context;
    private readonly IUserContext _userContext;
    
    public ArticleService(EnemContext context, IUserContext userContext)
    {
        _context = context;
        _userContext = userContext;
    }
    
    /// <inheritdoc/>
    public async Task<Result> CreateArticleAsync(PostUpdateArticleDto request, CancellationToken cancellationToken = default)
    {
        Guid userId = _userContext.UserId;

        var playlistIds = GetPlaylistIds(request.Blocks);

        if (playlistIds.Any())
        {
            var validPlaylistCount = await _context.Playlists
                .Where(p => (p.IsPublic || p.UserId == userId) && playlistIds.Contains(p.PlaylistId))
                .CountAsync(cancellationToken);

            if (validPlaylistCount != playlistIds.Count)
            {
                return Result.Failure(ArticleErrors.InvalidPlaylistReference);
            }
        }

        string serializedBlocks = JsonSerializer.Serialize(request.Blocks, SharedJsonOptions.Default);

        var article = new Article
        {
            ArticleId = Guid.CreateVersion7(),
            UserId = userId,
            Title = request.Title,
            Subtitle = request.Subtitle,
            BlocksJson = serializedBlocks,
        };

        foreach (var playlistId in playlistIds)
        {
            article.ArticlePlaylists.Add(new ArticlePlaylist
            {
                ArticleId = article.ArticleId,
                PlaylistId = playlistId
            });
        }

        _context.Articles.Add(article);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    /// <inheritdoc/>
    public async Task<Result> UpdateArticleAsync(Guid articleId, PostUpdateArticleDto request, CancellationToken cancellationToken = default)
    {
        Guid userId = _userContext.UserId;

        var article = await _context.Articles.SingleOrDefaultAsync(e => 
            e.ArticleId == articleId && e.UserId == userId, cancellationToken);

        if (article is null)
        {
            return Result.Failure(ArticleErrors.ArticleWasNotFound(articleId));
        }
        
        var playlistIds = GetPlaylistIds(request.Blocks);

        if (playlistIds.Any())
        {
            var validPlaylistCount = await _context.Playlists
                .Where(p => (p.IsPublic || p.UserId == userId) && playlistIds.Contains(p.PlaylistId))
                .CountAsync(cancellationToken);

            if (validPlaylistCount != playlistIds.Count)
            {
                return Result.Failure(ArticleErrors.InvalidPlaylistReference);
            }
        }
        
        string serializedBlocks = JsonSerializer.Serialize(request.Blocks, SharedJsonOptions.Default);

        article.Title = request.Title;
        article.Subtitle = request.Subtitle;
        article.BlocksJson = serializedBlocks;
        
        foreach (var playlistId in playlistIds)
        {
            article.ArticlePlaylists.Add(new ArticlePlaylist
            {
                ArticleId = article.ArticleId,
                PlaylistId = playlistId
            });
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    /// <inheritdoc/>
    public async Task<PagedResponse<GetArticleDto>> FetchArticlesAsync(ArticleQueryFilter filter, CancellationToken cancellationToken = default)
    {
        var pageNumber = Math.Max(1, filter.PageNumber);
        var pageSize = Math.Clamp(filter.PageSize, 1, 50);

        var query = _context.Articles
            .AsNoTracking()
            .AsQueryable();

        query = query.ApplySearch(filter.SearchTerm);

        var totalRecords = await query.CountAsync(cancellationToken);

        query = query.ApplySort(string.IsNullOrEmpty(filter.SortBy) ? "CreatedAt desc" : filter.SortBy);

        var articles = await query
            .ApplyPagination(pageNumber, pageSize)
            .Select(e => e.ToGetArticleDto())
            .ToListAsync(cancellationToken);

        return new PagedResponse<GetArticleDto>
        {
            Data = articles,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalRecords = totalRecords,
            TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize)
        };
    }

    /// <inheritdoc/>
    public async Task<Result<GetArticleDetailDto>> FetchArticleByIdAsync(Guid articleId, CancellationToken cancellationToken = default)
    {
        var article = await _context.Articles
            .AsNoTracking()
            .SingleOrDefaultAsync(e => e.ArticleId == articleId, cancellationToken);

        if (article is null)
        {
            return Result.Failure<GetArticleDetailDto>(ArticleErrors.ArticleWasNotFound(articleId));
        }

        return Result.Success(article.ToGetArticleDetailsDto());
    }

    /// <inheritdoc/>
    public async Task<Result> DeleteArticleAsync(Guid articleId, CancellationToken cancellationToken = default)
    {
        Guid userId = _userContext.UserId;

        var article = await _context.Articles
            .SingleOrDefaultAsync(e => e.ArticleId == articleId, cancellationToken);

        if (article is null)
        {
            return Result.Failure(ArticleErrors.ArticleWasNotFound(articleId));
        }

        if (article.UserId != userId)
        {
            return Result.Failure(ArticleErrors.CannotDeleteForbiddenAccess);
        }

        _context.Remove(article);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
    
    /// <summary>
    /// Fetches the playlist's ids from the Markdown Playlist blocks.
    /// </summary>
    /// <param name="blocks">The article Markdown blocks.</param>
    /// <returns>A Collection of Guids</returns>
    private ICollection<Guid> GetPlaylistIds(ICollection<ArticleBlock> blocks)
    {
        return blocks
            .OfType<PlaylistBlock>()
            .Select(p => p.PlaylistId)
            .Distinct()
            .ToList();
    }
}