using EnemPrep.Domain.Models;
using EnemPrep.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnemPrep.Persistence.Configurations;

public class SessionConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.ToTable(TableNames.Sessions, SchemaNames.Auth);

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Value)
            .HasColumnName("Value")
            .IsRequired();
        
        builder.Property(e => e.ExpiresAtTime)
            .HasColumnName("ExpiresAtTime")
            .IsRequired();

        builder.Property(e => e.SlidingExpirationInSeconds)
            .HasColumnName("SlidingExpirationInSeconds");
        
        builder.Property(e => e.AbsoluteExpiration)
            .HasColumnName("AbsoluteExpiration");
    }
}