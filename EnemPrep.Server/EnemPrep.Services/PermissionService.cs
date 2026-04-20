using EnemPrep.Domain.Models;
using EnemPrep.Persistence;
using EnemPrep.ServicesContracts;
using Microsoft.EntityFrameworkCore;

namespace EnemPrep.Services;

public class PermissionService : IPermissionService
{
    private readonly EnemContext _context;

    public PermissionService(EnemContext context)
    {
        _context = context;
    }

    public async Task<HashSet<string>> GetPermissionAsync(Guid userId)
    {
        ICollection<Role>[] roles = await _context.Set<User>()
            .Include(u => u.Roles)
            .ThenInclude(r => r.Permissions)
            .Where(u => u.UserId == userId)
            .Select(u => u.Roles).ToArrayAsync();

        return roles
            .SelectMany(x => x)
            .SelectMany(x => x.Permissions)
            .Select(x => x.Name)
            .ToHashSet();
    }
}