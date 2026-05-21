using EnemPrep.Domain.Common;
using EnemPrep.Domain.DTOS;
using EnemPrep.Domain.Result;

namespace EnemPrep.ServicesContracts;

public interface ISolvedQuestionsService
{
    Task<Result> SolveQuestionAsync(CreateSolvedQuestionDto request, CancellationToken cancellationToken = default);
    Task<Result<PagedResponse<SolvedQuestionDto>>> FetchSolvedQuestionsByUserIdAsync(Guid? userId, SolvedQuestionFilter filter, CancellationToken cancellationToken);
    Task<Result<SolvedQuestionDto>> FetchSolvedQuestionByIdAsync(Guid? id, CancellationToken cancellationToken = default);
}