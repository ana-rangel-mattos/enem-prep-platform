using EnemPrep.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Permission = EnemPrep.Domain.Enums.Permission;

namespace EnemPrep.Persistence.Configurations;

internal sealed class RolePermissionConfiguration 
    : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.HasKey(x => new 
            { x.RoleId, x.PermissionId });

        builder.HasData(
            // Student Permissions
            Create(Role.Student, Permission.ReadContent), 
            Create(Role.Student, Permission.ReadArticle), 
            Create(Role.Student, Permission.ReadPlaylist),
            Create(Role.Student, Permission.CreatePlaylist),
            Create(Role.Student, Permission.DeletePlaylist),
            Create(Role.Student, Permission.EditPlaylist),
            
            // Admin Permissions
            Create(Role.Admin, Permission.CreateQuestions), 
            Create(Role.Admin, Permission.DeleteQuestion), 
            Create(Role.Admin, Permission.DeleteUser),
            Create(Role.Admin, Permission.EditQuestion), 
            Create(Role.Admin, Permission.CreateSubject),
            Create(Role.Admin, Permission.DeleteSubject),
            
            // Teacher Permissions
            Create(Role.Teacher, Permission.ReadArticle),
            Create(Role.Teacher, Permission.CreateArticle), 
            Create(Role.Teacher, Permission.DeleteArticle), 
            Create(Role.Teacher, Permission.EditArticle), 
            Create(Role.Teacher, Permission.CreatePlaylist),
            Create(Role.Teacher, Permission.DeletePlaylist),
            Create(Role.Teacher, Permission.EditPlaylist),
            Create(Role.Teacher, Permission.ReadPlaylist)
        );
    }

    private static RolePermission Create(Role role, Permission permission)
    {
        return new RolePermission
        {
            RoleId = role.Id,
            PermissionId = (int)permission
        };
    }
}