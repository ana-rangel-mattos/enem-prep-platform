namespace EnemPrep.Domain.Models;

public partial class SavedQuestion
{
    public Guid UserId { get; set; }
    public Guid QuestionId { get; set; }
    public string? Notes { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public virtual User User { get; set; } = null!;
    public virtual Question Question { get; set; } = null!;
}