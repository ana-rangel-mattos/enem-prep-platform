using EnemPrep.Domain.Models;
using EnemPrep.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnemPrep.Persistence.Configurations;

public class SubjectConfiguration : IEntityTypeConfiguration<Subject>
{
    public void Configure(EntityTypeBuilder<Subject> builder)
    {
        builder.HasKey(e => e.SubjectId).HasName("subject_pkey");

        builder.ToTable(TableNames.Subjects, SchemaNames.Content);

        builder.Property(e => e.Name)
            .HasColumnName("name")
            .HasColumnType("content.subject_name");

        builder.Property(e => e.SubjectId)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("subject_id");
        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        builder.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
    }
}