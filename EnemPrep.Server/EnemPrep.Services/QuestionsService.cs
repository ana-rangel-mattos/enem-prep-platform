using System.Text.Json;
using EnemPrep.Domain.Common;
using EnemPrep.Domain.DTOS;
using EnemPrep.Persistence;
using EnemPrep.ServicesContracts;
using Microsoft.EntityFrameworkCore;

namespace EnemPrep.Services;

public class QuestionsService : IQuestionService
{
    private EnemContext _context;

    public QuestionsService(EnemContext context)
    {
        _context = context;
    }
    
    public async Task<QuestionDto?> FetchQuestionByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var foundQuestion = await _context.Questions.FirstOrDefaultAsync(q => q.QuestionId == id, cancellationToken);

        if (foundQuestion is null)
            return null;

        QuestionDto question = new QuestionDto
        {
            QuestionId = foundQuestion.QuestionId,
            ApiIndex = foundQuestion.ApiIndex,
            SubjectId = foundQuestion.SubjectId,
            CreatedAt = foundQuestion.CreatedAt,
            UpdatedAt = foundQuestion.UpdatedAt,
            Content = ParseJsonElement(foundQuestion.Content)
        };

        return question;
    }

    public async Task<PagedResponse<QuestionDto>> FetchQuestionsAsync(QuestionQueryFilter filter, CancellationToken cancellationToken = default)
    {
        var pageNumber = Math.Max(1, filter.PageNumber);
        var pageSize = Math.Clamp(filter.PageSize, 1, 50);

        var query = _context.Questions.AsNoTracking().AsQueryable();

        query = query.ApplySearch(filter.SearchTerm);

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