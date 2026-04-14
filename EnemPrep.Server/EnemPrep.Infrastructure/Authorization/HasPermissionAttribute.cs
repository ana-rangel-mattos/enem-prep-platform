using Microsoft.AspNetCore.Authorization;

namespace EnemPrep.Server.Authorization;

public sealed class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(Permission permission) : base(policy: permission.ToString())
    { }
}