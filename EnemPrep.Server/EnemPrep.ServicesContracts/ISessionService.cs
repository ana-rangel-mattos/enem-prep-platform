namespace EnemPrep.ServicesContracts;

public interface ISessionService
{
    void SetUserSession(Guid userId, string username);
    Guid? GetUserId();
    void RemoveUserSession();
}