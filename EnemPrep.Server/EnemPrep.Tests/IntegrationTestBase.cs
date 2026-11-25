using EnemPrep.Application.Features.Auth;
using EnemPrep.Domain.Enums;
using EnemPrep.Domain.Models;
using EnemPrep.Persistence;
using Microsoft.EntityFrameworkCore;

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

            var roleId = Role.Student.Id;

            var user = request.ConvertToUser();
            user.HashPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            
            context.Users.Add(user);
            await context.SaveChangesAsync();
            
            await context.Database.ExecuteSqlRawAsync(
                "INSERT INTO auth.\"RoleUser\" (\"RolesId\", \"UsersUserId\") VALUES ({0}, {1})",
                roleId, user.UserId);

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

    protected async Task<Schedule> SeedScheduleAsync(User owner)
    {
        using var scope = Factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<EnemContext>();
        
        var subject = await context.Subjects.FirstOrDefaultAsync();
        Guid subject1Guid = Guid.CreateVersion7();
        Guid subject2Guid = Guid.CreateVersion7();
        if (subject is null)
        {
            context.Subjects.Add(new Subject
            {
                SubjectId = subject1Guid,
                Name = SubjectName.Mathematics
            });
            context.Subjects.Add(new Subject
            {
                SubjectId = subject2Guid,
                Name = SubjectName.Humanities
            });
            
            await context.SaveChangesAsync();
        }

        var schedule = new Schedule
        {
            ScheduleId = Guid.CreateVersion7(),
            UserId = owner.UserId,
            Name = "Default Schedule",
            HasWeekEnd = true,
            CreatedAt = DateTime.UtcNow,
            ScheduleSubjects = new List<ScheduleSubject>
            {
                new() {SubjectId = subject2Guid, Weekday = DayOfTheWeek.Monday, SubjectOrder = 1},
                new() {SubjectId = subject1Guid, Weekday = DayOfTheWeek.Monday, SubjectOrder = 2},
                new() {SubjectId = subject2Guid, Weekday = DayOfTheWeek.Tuesday, SubjectOrder = 1}
            }
        };

        context.Schedules.Add(schedule);
        await context.SaveChangesAsync();

        return schedule;
    }
}
