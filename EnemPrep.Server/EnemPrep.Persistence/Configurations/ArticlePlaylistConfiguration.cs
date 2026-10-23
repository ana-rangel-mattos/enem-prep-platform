using EnemPrep.Domain.Models;
using EnemPrep.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnemPrep.Persistence.Configurations;

public class ArticlePlaylistConfiguration : IEntityTypeConfiguration<ArticlePlaylist>
{
    public void Configure(EntityTypeBuilder<ArticlePlaylist> builder)
    {
        builder.HasKey(e => new { e.ArticleId, e.PlaylistId }).HasName("article_playlist_pk");

        builder.ToTable("article_playlist", SchemaNames.Content);

        builder.Property(e => e.ArticleId).HasColumnName("article_id");
        builder.Property(e => e.PlaylistId).HasColumnName("playlist_id");
        
        builder.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");

        builder.HasOne(d => d.Article)
            .WithMany(p => p.ArticlePlaylists)
            .HasForeignKey(d => d.ArticleId)
            .HasConstraintName("article_playlist_article_id_fkey");

        builder.HasOne(d => d.Playlist)
            .WithMany(p => p.ArticlePlaylists)
            .HasForeignKey(d => d.PlaylistId)
            .HasConstraintName("article_playlist_playlist_id_fkey");
    }
}