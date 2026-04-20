using EnemPrep.Domain.Models;
using EnemPrep.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnemPrep.Persistence.Configurations;

public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
{
    public void Configure(EntityTypeBuilder<Schedule> builder)
    {
        builder.HasKey(e => e.ScheduleId).HasName("schedule_pkey");

        builder.ToTable(TableNames.Schedule, SchemaNames.Planning);

        builder.Property(e => e.ScheduleId)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("schedule_id");
        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        builder.Property(e => e.Name)
            .HasMaxLength(50)
            .HasDefaultValueSql("'Cronograma Semanal'::character varying")
            .HasColumnName("name");
        builder.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        builder.Property(e => e.UserId).HasColumnName("user_id");

        builder.HasOne(d => d.User).WithMany(p => p.Schedules)
            .HasForeignKey(d => d.UserId)
            .HasConstraintName("fk_user_id");
    }
}