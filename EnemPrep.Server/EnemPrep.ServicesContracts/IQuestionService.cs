using EnemPrep.Domain.Common;
using EnemPrep.Domain.DTOS;

namespace EnemPrep.ServicesContracts;

public interface IQuestionService
{
    Task<QuestionDto?> FetchQuestionByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedResponse<QuestionDto>> FetchQuestionsAsync(QuestionQueryFilter filter, CancellationToken cancellationToken = default);
}