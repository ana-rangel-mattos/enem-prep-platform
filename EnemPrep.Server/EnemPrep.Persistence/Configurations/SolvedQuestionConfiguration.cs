using EnemPrep.Domain.Models;
using EnemPrep.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnemPrep.Persistence.Configurations;

public class SolvedQuestionConfiguration : IEntityTypeConfiguration<SolvedQuestion>
{
    public void Configure(EntityTypeBuilder<SolvedQuestion> builder)
    {
        builder.HasKey(e => e.SolvedQuestionId).HasName("solved_question_pkey");

        builder.ToTable(TableNames.SolvedQuestions, SchemaNames.Tracking);

        builder.Property(e => e.SolvedQuestionId)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("solved_question_id");
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
        builder.Property(e => e.QuestionId).HasColumnName("question_id");
        builder.Property(e => e.QuestionYear).HasColumnName("question_year");
        builder.Property(e => e.TimeSpent).HasColumnName("time_spent");
        builder.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        builder.Property(e => e.UserId).HasColumnName("user_id");

        builder.HasOne(d => d.Question).WithMany(p => p.SolvedQuestions)
            .HasForeignKey(d => d.QuestionId)
            .HasConstraintName("solved_question_question_id_fkey");

        builder.HasOne(d => d.User).WithMany(p => p.SolvedQuestions)
            .HasForeignKey(d => d.UserId)
            .HasConstraintName("solved_question_user_id_fkey");
    }
}