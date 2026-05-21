using EnemPrep.Domain.Models;
using EnemPrep.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnemPrep.Persistence.Configurations;

public class SavedQuestionConfiguration : IEntityTypeConfiguration<SavedQuestion>
{
    public void Configure(EntityTypeBuilder<SavedQuestion> builder)
    {
        builder.HasKey(e => new { e.UserId, e.QuestionId })
            .HasName("saved_question_pkey");

        builder.ToTable(TableNames.SavedQuestions, SchemaNames.Auth);

        builder.HasOne(d => d.User)
            .WithMany(p => p.SavedQuestions)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fk_user_id");

        builder.HasOne(d => d.Question)
            .WithMany(p => p.SavedQuestions)
            .HasForeignKey(d => d.QuestionId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_question_id");

        builder.Property(e => e.UserId)
            .HasColumnName("user_id")
            .IsRequired();
        
        builder.Property(e => e.QuestionId)
            .HasColumnName("question_id")
            .IsRequired();
        
        builder.Property(e => e.Notes)
            .HasMaxLength(200)
            .HasColumnName("notes");

        builder.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        
        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
    }
}