using EnemPrep.Domain.Models;
using EnemPrep.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnemPrep.Persistence.Configurations;

public class InvitationCodeConfiguration 
    : IEntityTypeConfiguration<InvitationCode>
{
    public void Configure(EntityTypeBuilder<InvitationCode> builder)
    {
        builder.HasKey(e => e.InvitationCodeId)
            .HasName("invitation_code_pkey");

        builder.ToTable(TableNames.InvitationCode, SchemaNames.Auth);

        builder.Property(e => e.InvitationCodeId)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("invitation_code_id");

        builder.Property(e => e.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(e => e.Code)
            .IsUnique();

        builder.Property(e => e.IsUsed)
            .HasDefaultValue(false)
            .HasColumnName("is_used");

        builder.HasOne(d => d.InviteRole)
            .WithMany(p => p.InvitationCodes)
            .HasForeignKey(d => d.RoleId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_role_id");
        
        builder.HasOne(d => d.CreatedBy)
            .WithMany(p => p.CreatedInvitations)
            .HasForeignKey(d => d.CreatedById)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fk_created_by_id");
        
        builder.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");

        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
    }
}