using System.Text.Json;
using EnemPrep.Domain.Common;
using EnemPrep.Domain.DTOS;
using EnemPrep.Domain.Models;
using EnemPrep.Domain.Result;
using EnemPrep.Persistence;
using EnemPrep.ServicesContracts;
using Microsoft.EntityFrameworkCore;

namespace EnemPrep.Services;

public class QuestionsService : IQuestionService
{
    private EnemContext _context;
    private readonly ISessionService _sessionService;

    public QuestionsService(EnemContext context, ISessionService sessionService)
    {
        _context = context;
        _sessionService = sessionService;
    }
    
    public async Task<Result<QuestionDto>> FetchQuestionByIdAsync(Guid? id, CancellationToken cancellationToken = default)
    {
        if (id is null)
        {
            return Result.Failure<QuestionDto>(QuestionErrors.QuestionNullId);
        }
        
        var foundQuestion = await _context.Questions.FirstOrDefaultAsync(q => q.QuestionId == id, cancellationToken);

        if (foundQuestion is null)
        {
            return Result.Failure<QuestionDto>(QuestionErrors.QuestionNotFound(id));
        }

        QuestionDto question = new QuestionDto
        {
            QuestionId = foundQuestion.QuestionId,
            ApiIndex = foundQuestion.ApiIndex,
            SubjectId = foundQuestion.SubjectId,
            CreatedAt = foundQuestion.CreatedAt,
            UpdatedAt = foundQuestion.UpdatedAt,
            Content = ParseJsonElement(foundQuestion.Content)
        };

        return Result.Success(question);
    }

    public async Task<PagedResponse<QuestionDto>> FetchQuestionsAsync(QuestionQueryFilter filter, CancellationToken cancellationToken = default)
    {
        var pageNumber = Math.Max(1, filter.PageNumber);
        var pageSize = Math.Clamp(filter.PageSize, 1, 50);

        var query = _context.Questions.AsNoTracking().AsQueryable();

        query = query.ApplySearch(filter.SearchTerm);
        query = query.ApplySubjectFilter(filter.SubjectId);

        var totalRecords = await query.CountAsync(cancellationToken);

        query = query.ApplySort(string.IsNullOrWhiteSpace(filter.SortBy) ? "CreatedAt" : filter.SortBy);

        var rows = await query
            .ApplyPagination(pageNumber, pageSize)
            .Select(q => new QuestionListRow
            {
                QuestionId = q.QuestionId,
                ApiIndex = q.ApiIndex,
                SubjectId = q.SubjectId,
                CreatedAt = q.CreatedAt,
                UpdatedAt = q.UpdatedAt,
                Content = q.Content
            })
            .ToListAsync(cancellationToken);

        var questions = rows
            .Select(q => new QuestionDto
            {
                QuestionId = q.QuestionId,
                ApiIndex = q.ApiIndex,
                SubjectId = q.SubjectId,
                CreatedAt = q.CreatedAt,
                UpdatedAt = q.UpdatedAt,
                Content = ParseJsonElement(q.Content)
            })
            .ToList();

        return new PagedResponse<QuestionDto>
        {
            Data = questions,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalRecords = totalRecords,
            TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize)
        };
    }

    public async Task<Result> CreateQuestionAsync(PostQuestionDto request, CancellationToken cancellationToken = default)
    {
        if (_sessionService.GetUserId() != request.UploadedById)
        {
            return Result.Failure(QuestionErrors.PublisherIdDoesNotMatchLoggedUserId);
        }
        
        if (!request.UploadedById.HasValue)
        {
            return Result.Failure(QuestionErrors.NullPublisherId);
        }
        
        Question question = request.ToQuestion();
        question.PostedById = request.UploadedById.Value;

        await _context.Questions
            .AddAsync(question, cancellationToken);
        
        return Result.Success();
    }

    public async Task<Result> RemoveQuestionAsync(Guid? id, CancellationToken cancellationToken = default)
    {
        if (id is null)
        {
            return Result.Failure(QuestionErrors.QuestionNullId);
        }
        
        int deletedRows = await _context.Questions
            .Where(q => q.QuestionId == id)
            .ExecuteDeleteAsync(cancellationToken);

        if (deletedRows == 0)
        {
            return Result.Failure(QuestionErrors.QuestionNotFound(id));
        }

        return Result.Success();
    }

    public async Task<Result> SaveQuestionAsync(PostSavedQuestionDto request, CancellationToken cancellationToken = default)
    {
        Guid? userId = _sessionService.GetUserId();

        if (userId is null)
        {
            return Result.Failure(SavedQuestionErrors.UserIsNotLoggedIn);
        }

        Question? question = await _context.Questions.Where(q => q.QuestionId == request.QuestionId)
            .FirstOrDefaultAsync(cancellationToken);

        if (question is null)
        {
            return Result.Failure(SavedQuestionErrors.QuestionNotFound(request.QuestionId));
        }

        await _context.SavedQuestions.AddAsync(new SavedQuestion
        {
            QuestionId = request.QuestionId,
            UserId = userId.Value,
            Notes = request.Notes
        }, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> DeleteSavedQuestionAsync(Guid? savedQuestionId, CancellationToken cancellationToken = default)
    {
        Guid? userId = _sessionService.GetUserId();
        
        if (userId is null)
        {
            return Result.Failure(SavedQuestionErrors.UserIsNotLoggedIn);
        }

        int deletedRows = await _context.SavedQuestions.Where(q => q.QuestionId == savedQuestionId && q.UserId == userId.Value)
            .ExecuteDeleteAsync(cancellationToken);

        if (deletedRows == 0)
        {
            return Result.Failure(SavedQuestionErrors.SavedQuestionNotFound(savedQuestionId));
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<PagedResponse<GetSavedQuestionDto>> FetchSavedQuestions(SavedQuestionFilter filter,
        CancellationToken cancellationToken = default)
    {
        var pageNumber = Math.Max(1, filter.PageNumber);
        var pageSize = Math.Clamp(filter.PageSize, 1, 50);

        var query = _context.SavedQuestions.AsNoTracking().AsQueryable();

        query.ApplySearch(filter.SearchTerm);

        var totalRecords = await query.CountAsync(cancellationToken);

        query = query.ApplySort(string.IsNullOrWhiteSpace(filter.SortBy) ? "CreatedAt" : filter.SortBy);

        var savedQuestions = await query
            .ApplyPagination(pageNumber, pageSize)
            .Select(q => new GetSavedQuestionDto
            {
                SavedQuestionId = q.QuestionId,
                QuestionId = q.Question.QuestionId,
                Notes = q.Notes,
                QuestionText = GetQuestionText(q.Question.Content)
            })
            .ToListAsync(cancellationToken);

        return new PagedResponse<GetSavedQuestionDto>
        {
            Data = savedQuestions,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalRecords = totalRecords,
            TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize)
        };
    }
    
    private static string? GetQuestionText(string content)
    {
        return ParseJsonElement(content).TryGetProperty("context", out JsonElement enunciation)
            ? enunciation.GetString()
            : string.Empty;
    }

    private static JsonElement ParseJsonElement(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            using var emptyDocument = JsonDocument.Parse("{}");
            return emptyDocument.RootElement.Clone();
        }

        using var document = JsonDocument.Parse(json);
        return document.RootElement.Clone();
    }

    private sealed class QuestionListRow
    {
        public Guid QuestionId { get; init; }
        public int? ApiIndex { get; init; }
        public string? Content { get; init; }
        public Guid? SubjectId { get; init; }
        public DateTime? CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }
    }
}