namespace EnemPrep.ServicesContracts;

public interface IPermissionService
{
    Task<HashSet<string>> GetPermissionAsync(Guid userId);
}