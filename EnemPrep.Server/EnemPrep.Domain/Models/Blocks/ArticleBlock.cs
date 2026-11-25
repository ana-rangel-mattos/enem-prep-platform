using System.Text.Json.Serialization;

namespace EnemPrep.Domain.Models.Blocks;

public abstract class ArticleBlock
{
    [JsonPropertyName("id")] public string Id { get; set; } = Guid.CreateVersion7().ToString();
}

public class TextBlock : ArticleBlock
{
    [JsonPropertyName("content")] public string Content { get; set; } = string.Empty;
}

public class PlaylistBlock : ArticleBlock
{
    [JsonPropertyName("playlistId")]
    public Guid PlaylistId { get; set; }
}