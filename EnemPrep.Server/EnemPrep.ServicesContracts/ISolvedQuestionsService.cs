using EnemPrep.Domain.Common;
using EnemPrep.Domain.DTOS;
using EnemPrep.Domain.Result;

namespace EnemPrep.ServicesContracts;

public interface ISolvedQuestionsService
{
    Task<Result> SolveQuestionAsync(PostSolvedQuestionDto request, CancellationToken cancellationToken = default);
    Task<Result<PagedResponse<GetSolvedQuestionDto>>> FetchSolvedQuestionsByUserIdAsync(Guid? userId, SolvedQuestionFilter filter, CancellationToken cancellationToken);
    Task<Result<GetSolvedQuestionDto>> FetchSolvedQuestionByIdAsync(Guid? id, CancellationToken cancellationToken = default);
}