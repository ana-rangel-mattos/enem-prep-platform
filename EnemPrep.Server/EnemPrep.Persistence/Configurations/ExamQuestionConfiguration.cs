using EnemPrep.Domain.Models;
using EnemPrep.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnemPrep.Persistence.Configurations;

public class ExamQuestionConfiguration: IEntityTypeConfiguration<ExamQuestion>
{
    public void Configure(EntityTypeBuilder<ExamQuestion> builder)
    {
        builder.HasKey(e => new { e.ExamId, e.QuestionId }).HasName("exam_question_pk");

        builder.ToTable(TableNames.ExamQuestions, SchemaNames.Tracking);

        builder.Property(e => e.ExamId).HasColumnName("exam_id");
        builder.Property(e => e.QuestionId).HasColumnName("question_id");
        builder.Property(e => e.ChosenAlternative)
            .HasMaxLength(1)
            .HasColumnName("chosen_alternative");
        builder.Property(e => e.CorrectAlternative)
            .HasMaxLength(1)
            .HasColumnName("correct_alternative");
        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        builder.Property(e => e.IsCorrect).HasColumnName("is_correct");
        builder.Property(e => e.TimeSpent).HasColumnName("time_spent");
        builder.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");

        builder.HasOne(d => d.Exam).WithMany(p => p.ExamQuestions)
            .HasForeignKey(d => d.ExamId)
            .HasConstraintName("exam_question_exam_id_fkey");

        builder.HasOne(d => d.Question).WithMany(p => p.ExamQuestions)
            .HasForeignKey(d => d.QuestionId)
            .HasConstraintName("exam_question_question_id_fkey");
    }
}