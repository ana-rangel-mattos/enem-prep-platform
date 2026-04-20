using EnemPrep.Domain.Models;
using EnemPrep.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnemPrep.Persistence.Configurations;

public class ExamSubjectsConfiguration: IEntityTypeConfiguration<ExamSubject>
{
    public void Configure(EntityTypeBuilder<ExamSubject> builder)
    {
        builder.HasKey(e => new { e.SubjectId, e.ExamId }).HasName("pk_exam_subject");

        builder.ToTable(TableNames.ExamSubjects, SchemaNames.Tracking);

        builder.Property(e => e.SubjectId).HasColumnName("subject_id");
        builder.Property(e => e.ExamId).HasColumnName("exam_id");
        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        builder.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");

        builder.HasOne(d => d.Exam).WithMany(p => p.ExamSubjects)
            .HasForeignKey(d => d.ExamId)
            .HasConstraintName("exam_subject_exam_id_fkey");

        builder.HasOne(d => d.Subject).WithMany(p => p.ExamSubjects)
            .HasForeignKey(d => d.SubjectId)
            .OnDelete(DeleteBehavior.SetNull)
            .HasConstraintName("exam_subject_subject_id_fkey");
    }
}