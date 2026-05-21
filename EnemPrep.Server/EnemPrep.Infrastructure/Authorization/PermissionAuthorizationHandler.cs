using System.Security.Claims;
using EnemPrep.Persistence;
using EnemPrep.ServicesContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace EnemPrep.Infrastructure.Authorization;

public class PermissionAuthorizationHandler(IPermissionService permissionService) :
    AuthorizationHandler<PermissionRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (context.User.Identity?.IsAuthenticated != true)
        {
            return;
        }

        string? userIdValue = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!Guid.TryParse(userIdValue, out Guid userId))
        {
            return;
        }

        HashSet<string> permissions = await permissionService.GetPermissionAsync(userId);

        if (permissions.Contains(requirement.Permission))
        {
            context.Succeed(requirement);
        }
    }
}