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
    
    public async Task<Result> SolveQuestionAsync([FromBody] CreateSolvedQuestionDto request, CancellationToken cancellationToken = default)
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

    public async Task<Result<PagedResponse<SolvedQuestionDto>>> FetchSolvedQuestionsByUserIdAsync(Guid? userId, SolvedQuestionFilter filter, CancellationToken cancellationToken)
    {
        if (userId is null)
        {
            return Result.Failure<PagedResponse<SolvedQuestionDto>>(SolvedQuestionErrors.SolvedQuestionNullUserId);
        }
        
        var foundUser = await _context.Users
            .Where(u => u.UserId == userId)
            .Include(u => u.SolvedQuestions)
            .FirstOrDefaultAsync(cancellationToken);

        if (foundUser is null)
        {
            return Result.Failure<PagedResponse<SolvedQuestionDto>>(SolvedQuestionErrors.SolvedQuestionUserDoesNotExist(userId));
        }
        
        Guid? loggedUserId = _sessionService.GetUserId();

        if (foundUser.IsPrivate && loggedUserId != foundUser.UserId)
        {
            return Result.Failure<PagedResponse<SolvedQuestionDto>>(SolvedQuestionErrors.SolvedQuestionPrivateUser);
        }
        
        var pageNumber = Math.Max(1, filter.PageNumber);
        var pageSize = Math.Clamp(filter.PageSize, 1, 50);

        var query = _context.SolvedQuestions
            .AsNoTracking()
            .Where(q => q.UserId == userId)
            .AsQueryable();

        var totalRecords = await query.CountAsync(cancellationToken);
        
        query = query.ApplySort(string.IsNullOrWhiteSpace(filter.SortBy) ? "CreatedAt" : filter.SortBy);

        var questions = await query
            .ApplyPagination(pageNumber, pageSize)
            .Select(q => new SolvedQuestionDto
            {
                QuestionId = q.QuestionId,
                QuestionYear = q.QuestionYear,
                CorrectAlternative = q.CorrectAlternative,
                ChosenAlternative = q.ChosenAlternative,
                TimeSpent = q.TimeSpent,
                IsCorrect = q.IsCorrect,
                CreatedAt = q.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return Result.Success(new PagedResponse<SolvedQuestionDto>
        {
            Data = questions,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalRecords = totalRecords,
            TotalPages = (int) Math.Ceiling(totalRecords / (double) pageSize)
        });
    }

    public async Task<Result<SolvedQuestionDto>> FetchSolvedQuestionByIdAsync(Guid? id, CancellationToken cancellationToken = default)
    {
        if (id is null)
        {
            return Result.Failure<SolvedQuestionDto>(SolvedQuestionErrors.SolvedQuestionNullId);
        }
        
        SolvedQuestion? question = await _context.SolvedQuestions
            .Where(q => q.QuestionId == id)
            .Include(q => q.User)
            .FirstOrDefaultAsync(cancellationToken);

        if (question is null)
        {
            return Result.Failure<SolvedQuestionDto>(SolvedQuestionErrors.SolvedQuestionNotFound(id));
        }

        User foundUser = question.User;
        
        Guid? loggedUserId = _sessionService.GetUserId();

        if (!foundUser.IsPrivate || loggedUserId == foundUser.UserId)
        {
            SolvedQuestionDto dto = new SolvedQuestionDto
            {
                QuestionId = question.QuestionId,
                QuestionYear = question.QuestionYear,
                CorrectAlternative = question.CorrectAlternative,
                ChosenAlternative = question.ChosenAlternative,
                IsCorrect = question.IsCorrect,
                TimeSpent = question.TimeSpent
            };
            
            return Result.Success(dto);
        }

        return Result.Failure<SolvedQuestionDto>(SolvedQuestionErrors.SolvedQuestionPrivateUser);
    }
}