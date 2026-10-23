using EnemPrep.Domain.Models.Blocks;

namespace EnemPrep.Domain.DTOS;

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