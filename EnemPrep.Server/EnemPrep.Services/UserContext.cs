using System.Security.Authentication;
using EnemPrep.ServicesContracts;

namespace EnemPrep.Services;

public class UserContext : IUserContext
{
    private readonly ISessionService _sessionService;

    public UserContext(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }

    public Guid UserId => _sessionService.GetUserId() ?? throw new AuthenticationException("User execution context is unauthenticated.");
    public bool IsAuthenticated => _sessionService.GetUserId() is not null;
}