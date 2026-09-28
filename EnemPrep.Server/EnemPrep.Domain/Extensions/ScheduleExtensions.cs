using EnemPrep.Domain.DTOS;
using EnemPrep.Domain.Enums;
using EnemPrep.Domain.Models;

namespace EnemPrep.Domain.Extensions;

public static class ScheduleExtensions
{
    public static GetScheduleDto ToGetScheduleDto(this Schedule schedule)
    {
        return new GetScheduleDto
        {
            Name = schedule.Name,
            ScheduleId = schedule.ScheduleId
        };
    }

    public static GetScheduleDetailsDto ToGetScheduleDetailsDto(this Schedule schedule)
    {
        Dictionary<DayOfTheWeek, IReadOnlyCollection<GetScheduleSubjectDto>> subjects = new();

        for (int i = 0; i < (schedule.HasWeekEnd ? 7 : 6); i++)
        {
            subjects.Add((DayOfTheWeek) i, schedule.ScheduleSubjects.Where(ss => (int) ss.Weekday == i).OrderBy(ss => ss.SubjectOrder).Select(ss => ss.ToGetScheduleSubjectDto()).ToList());
        }
        
        return new GetScheduleDetailsDto
        {
            ScheduleId = schedule.ScheduleId,
            UserId = schedule.UserId,
            Name = schedule.Name,
            Schedule = subjects
        };
    }
}