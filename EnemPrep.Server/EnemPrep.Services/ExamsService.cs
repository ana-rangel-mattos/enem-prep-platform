using EnemPrep.Domain.Common;
using EnemPrep.Domain.DTOS;
using EnemPrep.Domain.Enums;
using EnemPrep.Domain.Extensions;
using EnemPrep.Domain.Models;
using EnemPrep.Domain.Result;
using EnemPrep.Persistence;
using EnemPrep.ServicesContracts;
using Microsoft.EntityFrameworkCore;

namespace EnemPrep.Services;

public class ExamsService(EnemContext context, IUserContext userContext) : IExamsService
{
    private readonly EnemContext _context = context;
    private readonly IUserContext _userContext = userContext;

    public async Task<Result<GetExamDto>> GenerateExamAsync(PostExamDto request, CancellationToken cancellationToken = default)
    {
        if (!request.Areas.Any())
        {
            return Result.Failure<GetExamDto>(ExamErrors.InvalidSubjects);
        }

        int questionsPerSubject = request.TotalQuestions / request.Areas.Count;
        int remainder = request.TotalQuestions % request.Areas.Count;

        var selectedQuestions = new List<Question>();

        foreach (var subject in request.Areas)
        {
            int amountToFetch = questionsPerSubject + (remainder > 0 ? 1 : 0);
            remainder--;

            List<Question> subjectQuestions = new List<Question>();

            if (subject == SubjectName.Languages && request.ExamLanguage is not null)
            {
                // (5 / 45) * 100 ≈ 11% of Foreign language questions
                int foreignLanguageAmount = (int)Math.Ceiling(amountToFetch * 0.11);
                amountToFetch -= foreignLanguageAmount;
                
                subjectQuestions.AddRange(await _context.Questions
                    .Where(q => q.Language == request.ExamLanguage)
                    .OrderBy(q => Guid.NewGuid())
                    .Take(foreignLanguageAmount)
                    .ToListAsync(cancellationToken));
                
                subjectQuestions = await _context.Questions
                    .Include(q => q.Subject)
                    .Where(q => q.Subject!.Name == subject && q.Language == null)
                    .OrderBy(q => Guid.NewGuid())
                    .Take(amountToFetch)
                    .ToListAsync(cancellationToken);
            }
            else
            {
                subjectQuestions = await _context.Questions
                    .Include(q => q.Subject)
                    .Where(q => q.Subject!.Name == subject && q.Language == null)
                    .OrderBy(q => Guid.NewGuid())
                    .Take(amountToFetch)
                    .ToListAsync(cancellationToken);   
            }

            if (subjectQuestions.Count < amountToFetch)
            {
                return Result.Failure<GetExamDto>(ExamErrors.NoEnoughQuestionsForSubject(subject.ToString()));
            }
            
            selectedQuestions.AddRange(subjectQuestions);
        }

        var finalQuestionList = selectedQuestions
            .OrderBy(_ => Guid.NewGuid())
            .ToList();

        string dateTimeNow = $"{DateTime.Now:dd/MM/yyyy HH:mm}";
        string defaultTitle = request.Areas.Count == 1
            ? $"Simulado: {request.Areas.First()} - {dateTimeNow}"
            : $"Simulado Geral - {dateTimeNow}";

        List<Subject> subjects = await _context.Subjects
            .Where(e => request.Areas.Contains(e.Name))
            .ToListAsync(cancellationToken);
        
        Guid examId = Guid.CreateVersion7();
        var exam = new Exam
        {
            ExamId = examId,
            UserId = _userContext.UserId,
            Title = defaultTitle,
            Status = ExamStatus.NotStarted,
            QuestionsCount = finalQuestionList.Count,
            ExamYear = DateTime.Now.Year,
            ExamQuestions = finalQuestionList.Select(question => new ExamQuestion 
            {
                ExamId = examId,
                QuestionId = question.QuestionId,
                CorrectAlternative = question.ExtractCorrectAlternative() ?? ' ',
            }).ToList(),
            ExamSubjects = subjects.Select(subject => new ExamSubject
            {
                SubjectId = subject.SubjectId,
            }).ToList()
        };

        _context.Exams.Add(exam);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(exam.ConvertToDto());
    }

    public async Task<Result> UpdateExamStatus(UpdateExamStatusDto request, CancellationToken cancellationToken = default)
    {
        var exam = await _context.Exams.Where(e => e.ExamId == request.ExamId)
            .FirstOrDefaultAsync(cancellationToken);

        if (exam is null)
        {
            return Result.Failure(ExamErrors.ExamWasNotFound(request.ExamId));
        }

        bool hasValidNewStatus = exam.Status.HasValidNewStatus(request.NewStatus);

        if (!hasValidNewStatus)
        {
            return Result.Failure(ExamErrors.InvalidNewStatus);
        }

        Guid userId = _userContext.UserId;

        int updatedRows = await _context.Exams
            .Where(e => e.ExamId == request.ExamId && e.UserId == userId)
            .ExecuteUpdateAsync(e => 
                e.SetProperty(x => x.Status, request.NewStatus), cancellationToken);

        if (updatedRows == 0)
        {
            return Result.Failure(ExamErrors.FailedToUpdateExamStatus);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result<TakeExamDto>> TakeExamAsync(Guid examId, CancellationToken cancellationToken = default)
    {
        Guid userId = _userContext.UserId;

        Exam? exam = await _context.Exams.Where(e => e.ExamId == examId)
            .Include(e => e.ExamQuestions)
            .ThenInclude(q => q.Question)
            .FirstOrDefaultAsync(cancellationToken);

        if (exam is null)
        {
            return Result.Failure<TakeExamDto>(ExamErrors.ExamWasNotFound(examId));
        }

        if (exam.UserId != userId)
        {
            return Result.Failure<TakeExamDto>(ExamErrors.ExamDoesNotBelongToLoggedUser);
        }

        return Result.Success(new TakeExamDto
        {
            ExamId = exam.ExamId,
            Title = exam.Title,
            Questions = exam.ExamQuestions.Select(q => new ExamQuestionDto
            {
                ExamQuestionId = q.QuestionId,
                Year = q.Question.ExtractYear(),
                Context = q.Question.ExtractContext()!,
                Enunciation = q.Question.ExtractEnunciation()!,
                Alternatives = q.Question.ExtractAlternatives()
            }).ToList()
        });
    }

    public async Task<Result> SubmitAnswerAsync(SubmitAnswerRequest request, CancellationToken cancellationToken = default)
    {
        Guid userId = _userContext.UserId;
        
        ExamQuestion? question = await _context.ExamQuestions
            .Where(q => q.ExamId == request.ExamId 
                        && q.QuestionId == request.QuestionId 
                        && q.Exam.UserId == userId)
            .FirstOrDefaultAsync(cancellationToken);

        if (question is null)
        {
            return Result.Failure(ExamErrors.ExamQuestionWasNotFound(request.QuestionId));
        }

        int updatedRows = await _context.ExamQuestions
            .Where(q => q.ExamId == request.ExamId 
                        && q.QuestionId == request.QuestionId 
                        && q.Exam.UserId == userId)
            .ExecuteUpdateAsync(q => q
                .SetProperty(e => e.ChosenAlternative, request.ChosenAlternative)
                .SetProperty(e => e.TimeSpent, TimeSpan.FromSeconds(request.TimeSpentInSeconds))
                .SetProperty(e => e.IsCorrect, e => e.CorrectAlternative == request.ChosenAlternative), 
                cancellationToken);

        if (updatedRows == 0)
        {
            return Result.Failure(ExamErrors.FailedToSubmitQuestionAnswer);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result<GetExamDto>> SubmitExam(Guid examId, CancellationToken cancellationToken = default)
    {
        Exam? exam = await _context.Exams.Where(e => e.ExamId == examId)
            .Include(e => e.ExamQuestions)
            .ThenInclude(e => e.Question.Subject)
            .FirstOrDefaultAsync(cancellationToken);

        if (exam is null)
        {
            return Result.Failure<GetExamDto>(ExamErrors.ExamWasNotFound(examId));
        }

        Guid userId = _userContext.UserId;

        if (exam.UserId != userId)
        {
            return Result.Failure<GetExamDto>(ExamErrors.ExamDoesNotBelongToLoggedUser);
        }

        TimeSpan totalSpentTime = TimeSpan.Zero;
        
        int correctQuestionsCount = 0;
        int unsolvedQuestionsCount = 0;

        int correctMath = 0;
        int correctNaturalSciences = 0;
        int correctHumanities = 0;
        int correctLanguages = 0;
        
        foreach (var examQuestion in exam.ExamQuestions)
        {
            if (examQuestion.ChosenAlternative is null)
            {
                unsolvedQuestionsCount++;
            }
            else if (examQuestion.IsCorrect)
            {
                correctQuestionsCount++;
                
                switch (examQuestion.Question.Subject?.Name)
                {
                    case SubjectName.Mathematics:
                        correctMath++;
                        break;
                    case SubjectName.NaturalSciences:
                        correctNaturalSciences++;
                        break;
                    case SubjectName.Humanities:
                        correctHumanities++;
                        break;
                    case SubjectName.Languages:
                        correctLanguages++;
                        break;
                }
            }

            totalSpentTime += examQuestion.TimeSpent;
        }

        int incorrectQuestionsCount = exam.QuestionsCount - correctQuestionsCount - unsolvedQuestionsCount;
        
        float finalScore = (ScoreCalculator.CalculateScore(correctMath, SubjectName.Mathematics) 
                            + ScoreCalculator.CalculateScore(correctNaturalSciences, SubjectName.NaturalSciences) 
                            + ScoreCalculator.CalculateScore(correctHumanities, SubjectName.Humanities)
                            + ScoreCalculator.CalculateScore(correctLanguages, SubjectName.Languages)) / exam.ExamSubjects.Count;

        int updatedRows = await _context.Exams
            .Where(e => e.ExamId == examId && e.UserId == userId)
            .ExecuteUpdateAsync(
                e => e
                    .SetProperty(q => q.Status, ExamStatus.Finished)
                    .SetProperty(q => q.CorrectQuestionsCount, correctQuestionsCount)
                    .SetProperty(q => q.IncorrectQuestionsCount, incorrectQuestionsCount)
                    .SetProperty(q => q.UnsolvedQuestionsCount, unsolvedQuestionsCount)
                    .SetProperty(q => q.TotalSpentTime, totalSpentTime)
                    .SetProperty(q => q.EstimatedScore, finalScore), 
                cancellationToken);

        if (updatedRows == 0)
        {
            return Result.Failure<GetExamDto>(ExamErrors.FailedToCompleteExam(examId));
        }

        await _context.SaveChangesAsync(cancellationToken);
        
        exam = await _context.Exams.Where(e => e.ExamId == examId)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (exam is null)
        {
            return Result.Failure<GetExamDto>(ExamErrors.ExamWasNotFound(examId));
        }

        return Result.Success(exam.ConvertToDto());
    }

    public async Task<PagedResponse<GetExamDto>> FetchAllExams(ExamQueryFilter filter, CancellationToken cancellationToken = default)
    {
        var pageNumber = Math.Max(1, filter.PageNumber);
        var pageSize = Math.Clamp(filter.PageSize, 1, 50);

        Guid userId = _userContext.UserId;

        var query = _context.Exams
            .AsNoTracking()
            .Where(e => e.UserId == userId && e.Status == ExamStatus.Finished)
            .AsQueryable();

        query = query.ApplySearch(filter.SearchTerm);

        var totalRecords = await query.CountAsync(cancellationToken);
        
        query = query.ApplySort(string.IsNullOrWhiteSpace(filter.SortBy) ? "CreatedAt" : filter.SortBy);

        var exams = await query
            .ApplyPagination(pageNumber, pageSize)
            .Select(e => e.ConvertToDto())
            .ToListAsync(cancellationToken);
        
        return new PagedResponse<GetExamDto>
        {
            Data = exams,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalRecords = totalRecords,
            TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize)
        };
    }
}