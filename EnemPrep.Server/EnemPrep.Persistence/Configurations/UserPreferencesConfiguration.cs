using EnemPrep.Domain.Enums;
using EnemPrep.Domain.Models;
using EnemPrep.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnemPrep.Persistence.Configurations;

public class UserPreferencesConfiguration: IEntityTypeConfiguration<UserPreference>
{
    public void Configure(EntityTypeBuilder<UserPreference> builder)
    {
        builder.HasKey(e => e.UserPreferencesId).HasName("user_preferences_pkey");

        builder.ToTable(TableNames.UserPreferences, SchemaNames.Auth);

        builder.Property(u => u.ColorScheme)
            .HasColumnName("color_scheme")
            .HasConversion(
                v => v == ColorScheme.OS ? "OS" :
                    v == ColorScheme.Dark ? "DARK" :
                    v == ColorScheme.Light ? "LIGHT" : "OS",
                v => v == "OS" ? ColorScheme.OS :
                    v == "DARK" ? ColorScheme.Dark :
                    v == "LIGHT" ? ColorScheme.Light : ColorScheme.OS
            );
        
            
        builder.Property(u => u.ExamLanguage)
            .HasColumnName("exam_language")
            .HasConversion(
                v => v == Language.English ? "INGLES" : 
                    v == Language.Spanish ? "ESPANHOL" : "INGLES",
                v => v == "INGLES" ? Language.English : 
                    v == "ESPANHOL" ? Language.Spanish : Language.English
            );

        builder.Property(e => e.UserPreferencesId)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("user_preferences_id");
        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        builder.Property(e => e.QuestionsPerDay)
            .HasDefaultValue(10)
            .HasColumnName("questions_per_day");
        builder.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        builder.Property(e => e.UserId).HasColumnName("user_id");

        builder.HasOne(d => d.User).WithMany(p => p.UserPreferences)
            .HasForeignKey(d => d.UserId)
            .HasConstraintName("fk_user_id");
    }
}