using EnemPrep.Domain.Enums;
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
            .HasConversion(
                v => v == SubjectName.Languages ? "linguagens" :
                    v == SubjectName.Humanities ? "ciencias-humanas" :
                    v == SubjectName.NaturalSciences ? "ciencias-natureza" :
                    v == SubjectName.Mathematics ? "matematica" : "Desconhecido",
                v => v == "linguagens" ? SubjectName.Languages :
                    v == "ciencias-humanas" ? SubjectName.Humanities :
                    v == "ciencias-natureza" ? SubjectName.NaturalSciences :
                    v == "matematica" ? SubjectName.Mathematics : SubjectName.Languages
            );

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