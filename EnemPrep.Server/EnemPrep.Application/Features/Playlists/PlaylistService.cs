using EnemPrep.Application.Common.Interfaces;
using EnemPrep.Application.Common.Models;
using EnemPrep.Application.Common.Querying;
using EnemPrep.Application.Features.Articles;
using EnemPrep.Domain.Models;
using EnemPrep.Domain.Result;
using EnemPrep.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EnemPrep.Application.Features.Playlists;

public class PlaylistService : IPlaylistService
{
    private readonly EnemContext _context;
    private readonly IUserContext _userContext;
    
    public PlaylistService(EnemContext context, IUserContext userContext)
    {
        _context = context;
        _userContext = userContext;
    }
    
    public async Task<PagedResponse<GetPlaylistDto>> FetchLoggedUserPlaylistsAsync(QueryFilter filter, CancellationToken cancellationToken = default)
    {
        Guid userId = _userContext.UserId;
        
        var pageNumber = Math.Max(1, filter.PageNumber);
        var pageSize = Math.Clamp(filter.PageSize, 1, 50);
        
        var query = _context.Playlists
            .AsNoTracking()
            .AsQueryable();

        var totalRecords = await query.CountAsync(cancellationToken);

        query = query.ApplySort(string.IsNullOrEmpty(filter.SortBy) ? "CreatedAt desc" : filter.SortBy);

        var playlists = await query
            .ApplyPagination(pageNumber, pageSize)
            .Select(e => e.ToGetPlaylistDto())
            .ToListAsync(cancellationToken);

        return new PagedResponse<GetPlaylistDto>
        {
            Data = playlists,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalRecords = totalRecords,
            TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize)
        };
    }

    public async Task<Result<GetPlaylistDetailsDto>> FetchPlaylistByIdAsync(Guid playlistId, CancellationToken cancellationToken = default)
    {
        Guid userId = _userContext.UserId;

        var playlist = await _context.Playlists
            .AsSplitQuery()
            .AsNoTracking()
            .Include(e => e.ArticlePlaylists)
            .ThenInclude(ap => ap.Article)
            .Include(e => e.PlaylistQuestions)
            .ThenInclude(pq => pq.Question)
            .SingleOrDefaultAsync(e => e.PlaylistId == playlistId, cancellationToken);

        if (playlist is null)
        {
            return Result.Failure<GetPlaylistDetailsDto>(PlaylistErrors.PlaylistWasNotFound(playlistId));
        }

        if (playlist.UserId != userId && !playlist.IsPublic)
        {
            return Result.Failure<GetPlaylistDetailsDto>(PlaylistErrors.PlaylistAccessForbidden);
        }

        return Result.Success(new GetPlaylistDetailsDto
        {
            UserId = playlist.UserId,
            PlaylistId = playlist.PlaylistId,
            Title = playlist.Title,
            Description = playlist.Description,
            IsPublic = playlist.IsPublic,
            Articles = [..playlist.ArticlePlaylists.Select(e => e.Article.ToGetArticleDto())],
            Questions = [..playlist.PlaylistQuestions.Select(e => e.ToGetPlaylistQuestionDto())]
        });
    }

    public async Task<Result> CreatePlaylistAsync(PostPlaylistDto request, CancellationToken cancellationToken = default)
    {
        Guid userId = _userContext.UserId;

        var playlist = new Playlist
        {
            UserId = userId,
            Title = request.Title,
            Description = request.Description,
            IsPublic = request.IsPublic
        };

        _context.Playlists.Add(playlist);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> DeletePlaylistAsync(Guid playlistId, CancellationToken cancellationToken = default)
    {
        Guid userId = _userContext.UserId;

        var playlist = await _context.Playlists
            .SingleOrDefaultAsync(e => e.PlaylistId == playlistId, cancellationToken);

        if (playlist is null)
        {
            return Result.Failure(PlaylistErrors.PlaylistWasNotFound(playlistId));
        }

        if (playlist.UserId != userId)
        {
            return Result.Failure(PlaylistErrors.PlaylistDeleteAccessForbidden);
        }

        _context.Playlists.Remove(playlist);
        await _context.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }

    public async Task<Result> AddQuestionToPlaylistAsync(Guid playlistId, Guid questionId, CancellationToken cancellationToken = default)
    {
        Guid userId = _userContext.UserId;
        
        var playlist = await _context.Playlists
            .SingleOrDefaultAsync(e => e.PlaylistId == playlistId, cancellationToken);

        if (playlist is null)
        {
            return Result.Failure(PlaylistErrors.PlaylistWasNotFound(playlistId));
        }

        if (playlist.UserId != userId)
        {
            return Result.Failure(PlaylistErrors.PlaylistAddQuestionAccessForbidden);
        }

        var question = await _context.Questions
            .AsNoTracking()
            .SingleOrDefaultAsync(e => e.QuestionId == questionId, cancellationToken);

        if (question is null)
        {
            return Result.Failure(PlaylistErrors.PlaylistQuestionNotFound(questionId));
        }
        
        playlist.PlaylistQuestions.Add(new PlaylistQuestion
        {
            QuestionId = question.QuestionId,
            PlaylistId = playlist.PlaylistId,
        });

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> RemoveQuestionFromPlaylistAsync(Guid playlistId, Guid questionId,
        CancellationToken cancellationToken)
    {
        Guid userId = _userContext.UserId;

        var playlist = await _context.Playlists
            .Include(e => e.PlaylistQuestions)
            .ThenInclude(p => p.Question)
            .SingleOrDefaultAsync(e => e.PlaylistId == playlistId, cancellationToken);

        if (playlist is null)
        {
            return Result.Failure(PlaylistErrors.PlaylistWasNotFound(playlistId));
        }

        if (playlist.UserId != userId)
        {
            return Result.Failure(PlaylistErrors.PlaylistDeleteQuestionAccessForbidden);
        }

        var playlistQuestion = playlist.PlaylistQuestions.SingleOrDefault(e => e.QuestionId == questionId);
        
        if (playlistQuestion is null)
        {
            return Result.Failure(PlaylistErrors.PlaylistQuestionNotFound(questionId));
        }

        _context.PlaylistQuestions.Remove(playlistQuestion);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}