using EnemPrep.Domain.Models;
using EnemPrep.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnemPrep.Persistence.Configurations;

public class ScheduleSubjectConfiguration : IEntityTypeConfiguration<ScheduleSubject>
{
    public void Configure(EntityTypeBuilder<ScheduleSubject> builder)
    {
        builder.HasKey(e => new { e.ScheduleId, e.SubjectId }).HasName("pk_schedule_subject");

        builder.ToTable(TableNames.ScheduleSubjects, SchemaNames.Planning);
            
        builder.Property(e => e.Weekday)
            .HasColumnName("weekday")
            .HasColumnType("planning.day_of_the_week");
            
        builder.Property(e => e.ScheduleId).HasColumnName("schedule_id");
        builder.Property(e => e.SubjectId).HasColumnName("subject_id");
        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        builder.Property(e => e.SubjectOrder).HasColumnName("subject_order");
        builder.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");

        builder.HasOne(d => d.Schedule).WithMany(p => p.ScheduleSubjects)
            .HasForeignKey(d => d.ScheduleId)
            .HasConstraintName("schedule_subject_schedule_id_fkey");

        builder.HasOne(d => d.Subject).WithMany(p => p.ScheduleSubjects)
            .HasForeignKey(d => d.SubjectId)
            .HasConstraintName("schedule_subject_subject_id_fkey");
    }
}