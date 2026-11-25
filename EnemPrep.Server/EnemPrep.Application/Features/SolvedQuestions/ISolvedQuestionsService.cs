using EnemPrep.Application.Common.Models;
using EnemPrep.Domain.Result;

namespace EnemPrep.Application.Features.SolvedQuestions;

public interface ISolvedQuestionsService
{
    Task<Result<Guid>> SolveQuestionAsync(PostSolvedQuestionDto request, CancellationToken cancellationToken = default);
    Task<Result<PagedResponse<GetSolvedQuestionDto>>> FetchSolvedQuestionsByUserIdAsync(Guid? userId, SolvedQuestionFilter filter, CancellationToken cancellationToken);
    Task<Result<GetSolvedQuestionDto>> FetchSolvedQuestionByIdAsync(Guid? id, CancellationToken cancellationToken = default);

    Task<PagedResponse<GetSolvedQuestionDto>> FetchUserSolvedQuestionsAsync(SolvedQuestionFilter filter,
        CancellationToken cancellationToken = default);
}