using EnemPrep.Domain.Models;
using EnemPrep.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnemPrep.Persistence.Configurations;

public class QuestionConfiguration: IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.HasKey(e => e.QuestionId).HasName("question_pkey");

        builder.ToTable(TableNames.Questions, SchemaNames.Content);

        builder.Property(e => e.Language)
            .HasColumnName("language")
            .HasColumnType("content.language");

        builder.Property(e => e.QuestionId)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("question_id");
        builder.Property(e => e.ApiIndex).HasColumnName("api_index");
        builder.Property(e => e.Content)
            .HasColumnType("jsonb")
            .HasColumnName("content");
        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        builder.Property(e => e.PostedById).HasColumnName("posted_by_id");
        builder.Property(e => e.SubjectId).HasColumnName("subject_id");
        builder.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");

        builder.HasOne(d => d.PostedBy).WithMany(p => p.Questions)
            .HasForeignKey(d => d.PostedById)
            .HasConstraintName("fk_user_id");

        builder.HasOne(d => d.Subject).WithMany(p => p.Questions)
            .HasForeignKey(d => d.SubjectId)
            .OnDelete(DeleteBehavior.SetNull)
            .HasConstraintName("fk_subject_id");
    }
}