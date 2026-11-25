using EnemPrep.Domain.Models;
using EnemPrep.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnemPrep.Persistence.Configurations;

public class PlaylistQuestionConfiguration : IEntityTypeConfiguration<PlaylistQuestion>
{
    public void Configure(EntityTypeBuilder<PlaylistQuestion> builder)
    {
        builder.HasKey(e => new { e.PlaylistId, e.QuestionId })
            .HasName("playlist_question_pk");

        builder.ToTable("playlist_question", SchemaNames.Content);
        
        builder.Property(e => e.PlaylistId).HasColumnName("playlist_id");
        builder.Property(e => e.QuestionId).HasColumnName("question_id");

        builder.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        
        builder.HasOne(d => d.Playlist)
            .WithMany(p => p.PlaylistQuestions)
            .HasForeignKey(d => d.PlaylistId)
            .HasConstraintName("playlist_question_playlist_id_fkey");

        builder.HasOne(d => d.Question)
            .WithMany(p => p.PlaylistQuestions)
            .HasForeignKey(d => d.QuestionId)
            .HasConstraintName("playlist_question_question_id_fkey");
    }
}