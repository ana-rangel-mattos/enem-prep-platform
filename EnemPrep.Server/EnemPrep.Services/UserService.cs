using EnemPrep.Domain.Result;
using EnemPrep.Persistence;
using EnemPrep.ServicesContracts;
using Microsoft.EntityFrameworkCore;

namespace EnemPrep.Services;

public class UserService(EnemContext context) : IUserService
{
    public async Task<Result> DeleteAccountAsync(Guid? userId, CancellationToken cancellationToken = default)
    {
        if (userId is null)
        {
            return Result.Failure(UserErrors.UserNullId);
        }

        var deletedRows = await context.Users
            .Where(u => u.UserId == userId)
            .ExecuteDeleteAsync(cancellationToken);

        if (deletedRows == 0)
        {
            return Result.Failure(UserErrors.UserNotFound(userId));
        }

        return Result.Success();
    }
}