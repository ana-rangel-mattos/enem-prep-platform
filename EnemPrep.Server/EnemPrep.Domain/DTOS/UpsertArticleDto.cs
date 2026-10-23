using EnemPrep.Domain.Models.Blocks;

namespace EnemPrep.Domain.DTOS;

public class UpsertArticleDto
{
    public string Title { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public List<ArticleBlock> Blocks { get; set; } = new();
}