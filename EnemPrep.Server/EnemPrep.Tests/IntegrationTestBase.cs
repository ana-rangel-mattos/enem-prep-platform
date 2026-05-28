using EnemPrep.Domain.DTOS;
using EnemPrep.Domain.Models;
using EnemPrep.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EnemPrep.Tests;

public abstract class IntegrationTestBase : IClassFixture<CustomWebApplicationFactory>, IAsyncLifetime
{
    protected readonly CustomWebApplicationFactory Factory;
    protected readonly HttpClient Client;

    protected IntegrationTestBase(CustomWebApplicationFactory factory)
    {
        Factory = factory;
        Client = factory.CreateClient();
    }
    
    public async Task InitializeAsync()
    {
        await Factory.ResetDatabaseAsync();
    }

    public Task DisposeAsync() => Task.CompletedTask;

    private async Task SeedAdminUser(EnemContext context)
    {
        if (!context.Users.Any(u => u.Username == "admin_test"))
        {
            var roleId = Role.Admin.Id;
            
            var admin = new User
            {
                Username = "admin_test",
                Email = "admin_test@tests.com",
                FullName = "Admin Test",
                HashPassword = BCrypt.Net.BCrypt.HashPassword("admin-pass123")
            };
            
            context.Users.Add(admin);
            await context.SaveChangesAsync();
            
            await context.Database.ExecuteSqlRawAsync(
                "INSERT INTO auth.\"RoleUser\" (\"RolesId\", \"UsersUserId\") VALUES ({0}, {1})",
                roleId, admin.UserId);
        }
    }

    protected async Task<User> SeedUserAsync(PostUserRequest request)
    {
        using (var scope = Factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<EnemContext>();

            var roleID = Role.Student.Id;

            var user = request.ConvertToUser();
            user.HashPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            
            context.Users.Add(user);
            await context.SaveChangesAsync();
            
            await context.Database.ExecuteSqlRawAsync(
                "INSERT INTO auth.\"RoleUser\" (\"RolesId\", \"UsersUserId\") VALUES ({0}, {1})",
                roleID, user.UserId);

            return user;
        }
    }

    protected async Task AuthenticateAsAdminAsync()
    {
        using (var scope = Factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<EnemContext>();
            await SeedAdminUser(context);
        }

        var loginRequest = new
            { Username = "admin_test", Password = "admin-pass123" };
        var response = await Client.PostAsJsonAsync("/api/auth/login", loginRequest);

        response.EnsureSuccessStatusCode();
    }

    private async Task<Role> GetRequiredRoleAsync(EnemContext context, Role expectedRole)
    {
        var role = await context.Set<Role>()
            .SingleOrDefaultAsync(role => role.Id == expectedRole.Id);

        if (role is not null)
        {
            return role;
        }

        var availableRoles = await context.Set<Role>()
            .Select(role => $"{role.Id}:{role.Name}")
            .OrderBy(role => role)
            .ToListAsync();

        throw new InvalidOperationException(
            $"Required role '{expectedRole.Name}' with ID '{expectedRole.Id}' was not found. Available roles: [{string.Join(", ", availableRoles)}].");
    }
    
    private static async Task<Role> GetRequiredTrackedRoleAsync(EnemContext context, Role expectedRole)
    {
        var localRole = context.Set<Role>()
            .Local
            .FirstOrDefault(role => role.Id == expectedRole.Id);

        if (localRole is not null)
        {
            context.Entry(localRole).State = EntityState.Unchanged;
            return localRole;
        }

        var role = await context.Set<Role>()
            .SingleOrDefaultAsync(role => role.Id == expectedRole.Id);

        if (role is null)
        {
            var availableRoles = await context.Set<Role>()
                .Select(role => $"{role.Id}:{role.Name}")
                .OrderBy(role => role)
                .ToListAsync();

            throw new InvalidOperationException(
                $"Required role '{expectedRole.Name}' with ID '{expectedRole.Id}' was not found. Available roles: [{string.Join(", ", availableRoles)}].");
        }

        context.Entry(role).State = EntityState.Unchanged;

        return role;
    }
}
