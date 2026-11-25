using EnemPrep.Application.Features.Auth;
using EnemPrep.Application.Features.Schedules;
using EnemPrep.Domain.Enums;
using EnemPrep.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EnemPrep.Tests.Api;

public class SchedulesControllerTests(CustomWebApplicationFactory factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task UpdateSubjectDay_WhenMovingToDifferentDay_ShouldShiftIndicesCorrectly()
    {
        var userRequest = new PostUserRequest()
        {
            Username = "schedule_testUsr",
            Email = "schedule@test.com",
            Password = "password123!",
            FullName = "Schedule Test User"
        };

        var user = await SeedUserAsync(userRequest);

        await Client.PostAsJsonAsync("/api/auth/login", new LoginUserRequest
        {
            Username = userRequest.Username,
            Password = userRequest.Password
        });

        var schedule = await SeedScheduleAsync(user);
        var targetSubject =
            schedule.ScheduleSubjects.First(s => s.Weekday == DayOfTheWeek.Monday && s.SubjectOrder == 1);

        var updateRequest = new UpdateScheduleSubjectOrderDto
        {
            ScheduleSubjectId = targetSubject.ScheduleSubjectId,
            NewDay = DayOfTheWeek.Tuesday,
            NewIndex = 1
        };

        var response = await Client.PutAsJsonAsync($"/api/schedules/{schedule.ScheduleId}/reorder", updateRequest);

        response.EnsureSuccessStatusCode();

        using var scope = factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<EnemContext>();

        var updatedSchedule = await context.Schedules
            .Include(e => e.ScheduleSubjects)
            .SingleAsync(s => s.ScheduleId == schedule.ScheduleId);

        var movedItem = updatedSchedule.ScheduleSubjects
            .Single(e => e.ScheduleSubjectId == targetSubject.ScheduleSubjectId);
        Assert.Equal(DayOfTheWeek.Tuesday, movedItem.Weekday);
        Assert.Equal(1, movedItem.SubjectOrder);

        var mondayRemaining = updatedSchedule.ScheduleSubjects
            .Single(e => e.Weekday == DayOfTheWeek.Monday);
        Assert.Equal(1, mondayRemaining.SubjectOrder);
    }
}