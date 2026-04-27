namespace EnemPrep.Domain.Models;

public partial class InvitationCode
{
    public Guid InvitationCodeId { get; set; }
    public Guid CreatedById { get; set; }
    public int RoleId { get; set; }
    
    public string Code { get; set; } = null!;
    public bool IsUsed { get; set; }
    
    public DateTime ExpiresAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime CreatedAt { get; set; }

    public virtual User CreatedBy { get; set; } = null!;
    public virtual Role InviteRole { get; set; } = null!;
}