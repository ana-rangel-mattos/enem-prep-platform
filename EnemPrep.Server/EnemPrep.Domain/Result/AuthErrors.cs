using EnemPrep.Persistence.Constants;

namespace EnemPrep.Domain.Result;

public static class AuthErrors
{
    public static Error UserNotFound(string username) => new Error(
            ErrorNames.LoginUserNotFound,
            $"User with username {username} was not found.");

    public static Error UserNotFoundId(Guid userId) => new Error(
        ErrorNames.FetchLoggedUserUserNotFound,
        $"User with ID {userId} was not found.");

    public static readonly Error InvalidPassword = new Error(
        ErrorNames.LoginInvalidPassword,
        "Invalid password.");

    public static readonly Error FailedToLogout = new Error(
        ErrorNames.LogoutFailedToLogout,
        $"Failed to logout user."
        );
    
    public static readonly Error UserAlreadyExists = new Error(
        ErrorNames.RegisterUserAlreadyExists,
        $"User already exists, try using another username or email."
    );
    
    public static readonly Error InvalidInvitationCode = new Error(
        ErrorNames.RegisterInvalidInvitationCode,
        $"Invalid invitation code."
    );
    
    public static readonly Error FailedRegister = new Error(
        ErrorNames.RegisterFailedRegister,
        $"Failed to register user, please try again."
    );
}