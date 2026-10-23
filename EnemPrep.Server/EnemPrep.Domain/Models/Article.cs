namespace EnemPrep.Domain.Models;

public partial class Article
{
    public Guid ArticleId { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public string BlocksJson { get; set; } = "[]";
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public virtual User User { get; set; } = null!;
    public virtual ICollection<ArticlePlaylist> ArticlePlaylists { get; set; } = new List<ArticlePlaylist>();
}