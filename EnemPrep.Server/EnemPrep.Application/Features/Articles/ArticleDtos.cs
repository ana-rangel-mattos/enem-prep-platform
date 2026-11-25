using EnemPrep.Domain.Models.Blocks;

namespace EnemPrep.Application.Features.Articles;

public class GetArticleDetailDto
{
    public Guid ArticleId { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public List<ArticleBlock> Blocks { get; set; } = new();
    public DateTime UpdatedAt { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class GetArticleDto
{
    public Guid ArticleId { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class PostUpdateArticleDto
{
    public string Title { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public List<ArticleBlock> Blocks { get; set; } = new();
}