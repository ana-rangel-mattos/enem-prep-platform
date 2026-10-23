using EnemPrep.Domain.Models;
using EnemPrep.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnemPrep.Persistence.Configurations;

public class ArticleConfiguration : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder.HasKey(e => e.ArticleId).HasName("article_pkey");

        builder.ToTable("article", SchemaNames.Content);

        builder.Property(e => e.ArticleId)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("article_id");
        builder.Property(e => e.UserId).HasColumnName("user_id");

        builder.Property(e => e.Title)
            .HasMaxLength(50)
            .HasColumnName("title")
            .IsRequired();
        builder.Property(e => e.Subtitle)
            .HasMaxLength(100)
            .HasColumnName("subtitle")
            .IsRequired();
        builder.Property(e => e.BlocksJson)
            .HasColumnType("jsonb")
            .HasColumnName("blocks_json")
            .IsRequired();
        
        builder.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");

        builder.HasOne(d => d.User)
            .WithMany(p => p.Articles)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fk_user_id");
    }
}