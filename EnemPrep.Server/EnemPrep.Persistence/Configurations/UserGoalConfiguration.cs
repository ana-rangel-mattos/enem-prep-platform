using EnemPrep.Domain.Models;
using EnemPrep.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnemPrep.Persistence.Configurations;

public class UserGoalConfiguration: IEntityTypeConfiguration<UserGoal>
{
    public void Configure(EntityTypeBuilder<UserGoal> builder)
    {
        builder.HasKey(e => e.UserGoalId).HasName("user_goal_pkey");

        builder.ToTable(TableNames.UserGoal, SchemaNames.Planning);

        builder.HasIndex(e => e.UserId, "unique_goal").IsUnique();

        builder.Property(e => e.UserGoalId)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("user_goal_id");
        builder.Property(e => e.CourseName)
            .HasMaxLength(255)
            .HasColumnName("course_name");
        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        builder.Property(e => e.CutOffScore).HasColumnName("cut_off_score");
        builder.Property(e => e.UniversityName)
            .HasMaxLength(255)
            .HasColumnName("university_name");
        builder.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        builder.Property(e => e.UserId).HasColumnName("user_id");

        builder.HasOne(d => d.User).WithOne(p => p.UserGoal)
            .HasForeignKey<UserGoal>(d => d.UserId)
            .HasConstraintName("fk_user_id");
    }
}