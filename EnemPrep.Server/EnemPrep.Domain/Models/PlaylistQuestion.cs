namespace EnemPrep.Domain.Models;

public partial class PlaylistQuestion
{
    public Guid PlaylistId { get; set; }
    public Guid QuestionId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public virtual Playlist Playlist { get; set; } = null!;
    public virtual Question Question { get; set; } = null!;
}