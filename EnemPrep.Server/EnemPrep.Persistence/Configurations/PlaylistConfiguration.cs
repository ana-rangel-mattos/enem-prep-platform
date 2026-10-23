using EnemPrep.Domain.Models;
using EnemPrep.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnemPrep.Persistence.Configurations;

public class PlaylistConfiguration : IEntityTypeConfiguration<Playlist>
{
    public void Configure(EntityTypeBuilder<Playlist> builder)
    {
        builder.HasKey(e => e.PlaylistId).HasName("playlist_pkey");

        builder.ToTable("playlist", SchemaNames.Content);

        builder.Property(e => e.PlaylistId)
            .HasColumnName("playlist_id")
            .HasDefaultValueSql("gen_random_uuid()");
        builder.Property(e => e.UserId)
            .HasColumnName("user_id");

        builder.Property(e => e.Title)
            .HasMaxLength(50)
            .HasColumnName("title")
            .IsRequired();
        builder.Property(e => e.Description)
            .HasMaxLength(500)
            .HasColumnName("description")
            .IsRequired();
        builder.Property(e => e.IsPublic)
            .HasColumnName("is_public")
            .HasDefaultValueSql("TRUE");
        
        builder.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");

        builder.HasOne(d => d.User)
            .WithMany(p => p.Playlists)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fk_user_id");
    }
}