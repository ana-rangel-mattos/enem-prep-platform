namespace EnemPrep.Application.Common.Interfaces;

public interface IPermissionService
{
    Task<HashSet<string>> GetPermissionAsync(Guid userId);
}