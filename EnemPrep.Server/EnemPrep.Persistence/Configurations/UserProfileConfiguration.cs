using EnemPrep.Domain.Models;
using EnemPrep.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnemPrep.Persistence.Configurations;

public class UserProfileConfiguration: IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.HasKey(e => e.UserId).HasName("user_profile_pkey");

        builder.ToTable(TableNames.UserProfile, SchemaNames.Tracking);

        builder.Property(e => e.UserId)
            .ValueGeneratedNever()
            .HasColumnName("user_id");
        builder.Property(e => e.CurrentLevel)
            .HasDefaultValue(1)
            .HasColumnName("current_level");
        builder.Property(e => e.ExperiencePoints)
            .HasDefaultValue(0)
            .HasColumnName("experience_points");
        builder.Property(e => e.LastActivityDate)
            .HasDefaultValueSql("now()")
            .HasColumnName("last_activity_date");
        builder.Property(e => e.StreakCount)
            .HasDefaultValue(0)
            .HasColumnName("streak_count");
        builder.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        builder.Property(e => e.UserBio)
            .HasMaxLength(400)
            .HasColumnName("user_bio");

        builder.HasOne(d => d.User).WithOne(p => p.UserProfile)
            .HasForeignKey<UserProfile>(d => d.UserId)
            .HasConstraintName("user_profile_user_id_fkey");
    }
}