using EnemPrep.Persistence;
using EnemPrep.ServicesContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace EnemPrep.Infrastructure.Authorization;

public class PermissionAuthorizationHandler : 
    AuthorizationHandler<PermissionRequirement>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ISessionService _sessionService;
    
    public PermissionAuthorizationHandler(IServiceScopeFactory serviceScopeFactory, ISessionService sessionService)
    {
        _sessionService = sessionService;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        Guid? userId = _sessionService.GetUserId();

        if (userId is null)
        {
            return;
        }

        using IServiceScope scope = _serviceScopeFactory.CreateScope();

        IPermissionService permissionService = scope.ServiceProvider.GetRequiredService<IPermissionService>();

        HashSet<string> permissions = await permissionService.GetPermissionAsync(userId.Value);

        if (permissions.Contains(requirement.Permission))
        {
            context.Succeed(requirement);
        }
    }
}