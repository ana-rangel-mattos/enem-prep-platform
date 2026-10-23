namespace EnemPrep.Domain.Models;

public partial class Playlist
{
    public Guid PlaylistId { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsPublic { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public virtual User User { get; set; } = null!;
    public virtual ICollection<PlaylistQuestion> PlaylistQuestions { get; set; } = new List<PlaylistQuestion>();
    public virtual ICollection<ArticlePlaylist> ArticlePlaylists { get; set; } = new List<ArticlePlaylist>();
}