using EnemPrep.Domain.Common;
using EnemPrep.Domain.DTOS;
using EnemPrep.Domain.Result;

namespace EnemPrep.ServicesContracts;

public interface IArticleService
{
    Task<Result> CreateArticleAsync(UpsertArticleDto request, CancellationToken cancellationToken = default);

    Task<PagedResponse<GetArticleDto>> FetchArticlesAsync(ArticleQueryFilter filter,
        CancellationToken cancellationToken = default);
    Task<Result<GetArticleDetailDto>> FetchArticleByIdAsync(Guid articleId, CancellationToken cancellationToken = default);
}