namespace EnemPrep.ServicesContracts;

public interface IUserContext
{
    Guid UserId { get; }
    bool IsAuthenticated { get; }
}