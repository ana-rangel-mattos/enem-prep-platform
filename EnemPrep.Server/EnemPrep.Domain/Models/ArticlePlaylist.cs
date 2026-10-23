namespace EnemPrep.Domain.Models;

public class ArticlePlaylist
{
    public Guid ArticleId { get; set; }
    public Guid PlaylistId { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public virtual Article Article { get; set; } = null!;
    public virtual Playlist Playlist { get; set; } = null!;
}