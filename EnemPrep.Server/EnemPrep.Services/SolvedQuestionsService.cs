using System.Text.Json;
using EnemPrep.Domain.Common;
using EnemPrep.Domain.DTOS;
using EnemPrep.Domain.Models;
using EnemPrep.Domain.Result;
using EnemPrep.Persistence;
using EnemPrep.ServicesContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnemPrep.Services;

public class SolvedQuestionsService : ISolvedQuestionsService
{
    private readonly EnemContext _context;
    private readonly ISessionService _sessionService;
    
    public SolvedQuestionsService(EnemContext context, ISessionService sessionService)
    {
        _context = context;
        _sessionService = sessionService;
    }
    
    public async Task<Result> SolveQuestionAsync([FromBody] PostSolvedQuestionDto request, CancellationToken cancellationToken = default)
    {
        var foundQuestion = await _context.Questions
            .FirstOrDefaultAsync(q => q.QuestionId == request.QuestionId, cancellationToken);

        if (foundQuestion is null)
        {
            return Result.Failure(SolvedQuestionErrors.SolvedQuestionQuestionNotFound(request.QuestionId));
        }
        
        Guid? loggedUserId = _sessionService.GetUserId();
        
        SolvedQuestion solvedQuestion = request.ToSolvedQuestion();
        solvedQuestion.UserId = loggedUserId!.Value;
        
        await _context.SolvedQuestions.AddAsync(solvedQuestion, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }

    public async Task<Result<PagedResponse<GetSolvedQuestionDto>>> FetchSolvedQuestionsByUserIdAsync(Guid? userId, SolvedQuestionFilter filter, CancellationToken cancellationToken)
    {
        if (userId is null)
        {
            return Result.Failure<PagedResponse<GetSolvedQuestionDto>>(SolvedQuestionErrors.SolvedQuestionNullUserId);
        }
        
        var foundUser = await _context.Users
            .Where(u => u.UserId == userId)
            .Include(u => u.SolvedQuestions)
            .ThenInclude(sq => sq.Question)
            .FirstOrDefaultAsync(cancellationToken);

        if (foundUser is null)
        {
            return Result.Failure<PagedResponse<GetSolvedQuestionDto>>(SolvedQuestionErrors.SolvedQuestionUserDoesNotExist(userId));
        }
        
        Guid? loggedUserId = _sessionService.GetUserId();

        if (foundUser.IsPrivate && loggedUserId != foundUser.UserId)
        {
            return Result.Failure<PagedResponse<GetSolvedQuestionDto>>(SolvedQuestionErrors.SolvedQuestionPrivateUser);
        }
        
        var pageNumber = Math.Max(1, filter.PageNumber);
        var pageSize = Math.Clamp(filter.PageSize, 1, 50);

        var query = _context.SolvedQuestions
            .AsNoTracking()
            .Where(q => q.UserId == userId)
            .AsQueryable();

        var totalRecords = await query.CountAsync(cancellationToken);
        
        query = query.ApplySort(string.IsNullOrWhiteSpace(filter.SortBy) ? "CreatedAt" : filter.SortBy);

        JsonElement alternativesElement;
        var questions = await query
            .ApplyPagination(pageNumber, pageSize)
            .Select(q => new GetSolvedQuestionDto
            {
                QuestionId = q.QuestionId,
                QuestionYear = q.QuestionYear,
                QuestionText = GetQuestionText(q.Question.Content),
                CorrectAlternative = q.CorrectAlternative,
                CorrectAlternativeText = GetAlternativeText(
                    q.Question.Content,
                    q.CorrectAlternative.ToString()),
                ChosenAlternative = q.ChosenAlternative,
                ChosenAlternativeText = GetAlternativeText(
                    q.Question.Content,
                    q.ChosenAlternative.ToString()),
                TimeSpent = q.TimeSpent,
                IsCorrect = q.IsCorrect,
                CreatedAt = q.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return Result.Success(new PagedResponse<GetSolvedQuestionDto>
        {
            Data = questions,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalRecords = totalRecords,
            TotalPages = (int) Math.Ceiling(totalRecords / (double) pageSize)
        });
    }

    public async Task<Result<GetSolvedQuestionDto>> FetchSolvedQuestionByIdAsync(Guid? id, CancellationToken cancellationToken = default)
    {
        if (id is null)
        {
            return Result.Failure<GetSolvedQuestionDto>(SolvedQuestionErrors.SolvedQuestionNullId);
        }
        
        SolvedQuestion? question = await _context.SolvedQuestions
            .Where(q => q.QuestionId == id)
            .Include(q => q.Question)
            .Include(q => q.User)
            .FirstOrDefaultAsync(cancellationToken);

        if (question is null)
        {
            return Result.Failure<GetSolvedQuestionDto>(SolvedQuestionErrors.SolvedQuestionNotFound(id));
        }

        User foundUser = question.User;
        
        Guid? loggedUserId = _sessionService.GetUserId();

        if (!foundUser.IsPrivate || loggedUserId == foundUser.UserId)
        {
            var questionText = GetQuestionText(question.Question.Content);
            
            var correctAlternative = question.CorrectAlternative.ToString();
            string? correctAlternativeText = GetAlternativeText(
                question.Question.Content,
                correctAlternative
            );
            
            var chosenAlternative = question.ChosenAlternative.ToString();
            string? chosenAlternativeText = correctAlternative.Equals(chosenAlternative)
                ? correctAlternativeText
                : GetAlternativeText(
                    question.Question.Content,
                    chosenAlternative
            );
            
            var dto = new GetSolvedQuestionDto
            {
                QuestionId = question.QuestionId,
                QuestionYear = question.QuestionYear,
                QuestionText = questionText,
                CorrectAlternative = question.CorrectAlternative,
                CorrectAlternativeText = correctAlternativeText,
                ChosenAlternative = question.ChosenAlternative,
                ChosenAlternativeText = chosenAlternativeText,
                IsCorrect = question.IsCorrect,
                TimeSpent = question.TimeSpent
            };
            
            return Result.Success(dto);
        }

        return Result.Failure<GetSolvedQuestionDto>(SolvedQuestionErrors.SolvedQuestionPrivateUser);
    }

    private static string? GetQuestionText(string content)
    {
        return ParseJsonElement(content).TryGetProperty("context", out JsonElement enunciation)
            ? enunciation.GetString()
            : string.Empty;
    }

    private static string? GetAlternativeText(string content, string? alternativeLetter)
    {
        var jsonContent = ParseJsonElement(content);
        var hasValidAlternatives = jsonContent.TryGetProperty("alternatives", out JsonElement alternatives);
        
        if (hasValidAlternatives && alternatives.ValueKind == JsonValueKind.Array)
        {
            foreach (var alternative in alternatives.EnumerateArray())
            {
                var letter = alternative.TryGetProperty("letter", out JsonElement letterElement)
                    ? letterElement.GetString()
                    : string.Empty;

                if (!string.IsNullOrEmpty(letter) && letter.Equals(alternativeLetter))
                {
                    return alternative.TryGetProperty("text", out JsonElement textElement)
                        ? textElement.GetString()
                        : string.Empty;
                }
            }
        }

        return string.Empty;
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
}