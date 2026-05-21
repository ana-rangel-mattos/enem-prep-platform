using EnemPrep.Domain.Common;
using EnemPrep.Domain.DTOS;
using EnemPrep.Domain.Result;

namespace EnemPrep.ServicesContracts;

public interface IQuestionService
{
    Task<Result<QuestionDto>> FetchQuestionByIdAsync(Guid? id, CancellationToken cancellationToken = default);
    Task<PagedResponse<QuestionDto>> FetchQuestionsAsync(QuestionQueryFilter filter, CancellationToken cancellationToken = default);
    Task<Result> CreateQuestionAsync(CreateQuestionDto request, CancellationToken cancellationToken = default);
    Task<Result> RemoveQuestionAsync(Guid? id, CancellationToken cancellationToken = default);
}