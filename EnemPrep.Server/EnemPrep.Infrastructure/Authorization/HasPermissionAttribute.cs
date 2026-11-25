using EnemPrep.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace EnemPrep.Infrastructure.Authorization;

public sealed class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(Permission permission)
        : base(policy: permission.ToString())
    {
        
    }
}