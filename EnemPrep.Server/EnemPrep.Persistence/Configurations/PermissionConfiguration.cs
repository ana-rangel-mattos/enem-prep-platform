using EnemPrep.Domain.Models;
using EnemPrep.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnemPrep.Persistence.Configurations;

internal sealed class PermissionConfiguration: IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable(TableNames.Permissions, SchemaNames.Auth);

        builder.HasKey(x => x.Id);

        IEnumerable<Permission> permissions = Enum.GetValues<Domain.Enums.Permission>()
            .Select(p => new Permission
            {
                Id = (int) p, 
                Name = p.ToString()
            });

        builder.HasData(permissions);
    }
}