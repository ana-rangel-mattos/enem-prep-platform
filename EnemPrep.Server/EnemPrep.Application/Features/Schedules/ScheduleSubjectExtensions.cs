using EnemPrep.Domain.Models;

namespace EnemPrep.Application.Features.Schedules;

public static class ScheduleSubjectExtensions
{
    public static GetScheduleSubjectDto ToGetScheduleSubjectDto(this ScheduleSubject subject)
    {
        return new GetScheduleSubjectDto
        {
            ScheduleSubjectId = subject.ScheduleSubjectId,
            SubjectName = subject.Subject.Name.ToString(),
            OrderIndex = subject.SubjectOrder
        };
    }
}