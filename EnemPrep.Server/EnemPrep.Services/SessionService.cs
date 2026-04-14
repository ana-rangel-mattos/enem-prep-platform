using EnemPrep.ServicesContracts;
using Microsoft.AspNetCore.Http;

namespace EnemPrep.Services;

public class SessionService : ISessionService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private const string UserIdKey = "CurrentUserId";
    private const string UsernameKey = "CurrentUsername";

    public SessionService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public void SetUserSession(Guid userId, string username)
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        if (session is not null)
        {
            session.SetString(UserIdKey, userId.ToString());
            session.SetString(UsernameKey, username);
        }
    }

    public Guid? GetUserId()
    {
        var userIdStr = _httpContextAccessor.HttpContext?.Session.GetString(UserIdKey);

        bool successfullyConverted = Guid.TryParse(userIdStr, out var userId);

        if (!successfullyConverted) return null;

        return userId;
    }

    public void RemoveUserSession()
    {
        _httpContextAccessor.HttpContext?.Session.Clear();
    }
}