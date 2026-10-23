using System.Text.Json;
using EnemPrep.Domain.Common;
using EnemPrep.Domain.DTOS;
using EnemPrep.Domain.Models;
using EnemPrep.Domain.Models.Blocks;

namespace EnemPrep.Domain.Extensions;

public static class ArticleExtensions
{
    public static GetArticleDto ToGetArticleDto(this Article article)
    {
        return new GetArticleDto
        {
            UserId = article.UserId,
            ArticleId = article.ArticleId,
            Title = article.Title,
            Subtitle = article.Subtitle,
            CreatedAt = article.CreatedAt,
            UpdatedAt = article.UpdatedAt
        };
    }
    
    public static GetArticleDetailDto ToGetArticleDetailsDto(this Article article)
    {
        var blocks = string.IsNullOrEmpty(article.BlocksJson)
            ? new List<ArticleBlock>()
            : JsonSerializer.Deserialize<List<ArticleBlock>>(article.BlocksJson, SharedJsonOptions.Default)
              ?? new();

        return new GetArticleDetailDto
        {
            ArticleId = article.ArticleId,
            UserId = article.UserId,
            Title = article.Title,
            Subtitle = article.Subtitle,
            Blocks = blocks,
            UpdatedAt = article.UpdatedAt,
            CreatedAt = article.CreatedAt
        };
    }
}