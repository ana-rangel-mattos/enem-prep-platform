using EnemPrep.Domain.Models;
using EnemPrep.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnemPrep.Persistence.Configurations;

public class UserConfiguration: IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(e => e.UserId).HasName("user_pkey");
        
        builder.ToTable(TableNames.Users, SchemaNames.Auth);
        
        builder.Ignore("Role");
        
        builder
            .HasMany(u => u.Roles)
            .WithMany(r => r.Users);
            
        builder.HasIndex(e => e.Username, "user_username_key").IsUnique();

        builder.Property(e => e.UserId)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("user_id");
        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        builder.Property(e => e.DateOfBirth)
            .HasColumnType("timestamp without time zone")
            .HasColumnName("date_of_birth");
        builder.Property(e => e.Email)
            .HasMaxLength(320)
            .HasColumnName("email");
        builder.Property(e => e.FullName)
            .HasMaxLength(255)
            .HasColumnName("full_name");
        builder.Property(e => e.HashPassword).HasColumnName("hash_password");
        builder.Property(e => e.IsPrivate)
            .HasDefaultValue(false)
            .HasColumnName("is_private");
        builder.Property(e => e.ProfileImage).HasColumnName("profile_image");
        builder.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        builder.Property(e => e.Username)
            .HasMaxLength(20)
            .HasColumnName("username");
    }
}