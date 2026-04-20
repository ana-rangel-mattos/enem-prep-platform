using EnemPrep.Domain.Models;
using EnemPrep.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnemPrep.Persistence.Configurations;

public class ExamConfiguration: IEntityTypeConfiguration<Exam>
{
    public void Configure(EntityTypeBuilder<Exam> builder)
    {
        builder.HasKey(e => e.ExamId).HasName("exam_pkey");

        builder.ToTable(TableNames.Exams, SchemaNames.Tracking);
            
        builder.Property(e => e.Status)
            .HasColumnName("status")
            .HasColumnType("tracking.exam_status");
            
        builder.Property(e => e.LanguageChoice)
            .HasColumnName("language_choice")
            .HasColumnType("content.language");

        builder.Property(e => e.ExamId)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("exam_id");
        builder.Property(e => e.CorrectQuestionsCount).HasColumnName("correct_questions_count");
        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        builder.Property(e => e.EstimatedScore).HasColumnName("estimated_score");
        builder.Property(e => e.ExamYear).HasColumnName("exam_year");
        builder.Property(e => e.IncorrectQuestionsCount).HasColumnName("incorrect_questions_count");
        builder.Property(e => e.MaxSpentTime).HasColumnName("max_spent_time");
        builder.Property(e => e.QuestionsCount).HasColumnName("questions_count");
        builder.Property(e => e.Title)
            .HasMaxLength(20)
            .HasColumnName("title");
        builder.Property(e => e.TotalSpentTime).HasColumnName("total_spent_time");
        builder.Property(e => e.UnsolvedQuestionsCount).HasColumnName("unsolved_questions_count");
        builder.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        builder.Property(e => e.UserId).HasColumnName("user_id");

        builder.HasOne(d => d.User).WithMany(p => p.Exams)
            .HasForeignKey(d => d.UserId)
            .HasConstraintName("fk_user_id");
    }
}