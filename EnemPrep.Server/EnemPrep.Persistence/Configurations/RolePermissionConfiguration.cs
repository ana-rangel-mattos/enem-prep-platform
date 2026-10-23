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

        builder.HasData(Create(Role.Student, Permission.ReadContent), Create(Role.Admin, Permission.CreateQuestions), Create(Role.Admin, Permission.DeleteQuestion), Create(Role.Admin, Permission.DeleteUser), Create(Role.Admin, Permission.EditQuestion), Create(Role.Teacher, Permission.CreateArticle), Create(Role.Teacher, Permission.DeleteArticle), Create(Role.Teacher, Permission.EditArticle), Create(Role.Teacher, Permission.CreateExerciseList), Create(Role.Teacher, Permission.EditExerciseList), Create(Role.Teacher, Permission.DeleteExerciseList));
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