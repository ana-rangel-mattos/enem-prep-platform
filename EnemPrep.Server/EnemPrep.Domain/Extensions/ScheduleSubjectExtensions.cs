using EnemPrep.Domain.DTOS;
using EnemPrep.Domain.Models;

namespace EnemPrep.Domain.Extensions;

public static class ScheduleSubjectExtensions
{
    public static GetScheduleSubjectDto ToGetScheduleSubjectDto(this ScheduleSubject subject)
    {
        return new GetScheduleSubjectDto
        {
            Weekday = subject.Weekday,
            OrderIndex = subject.SubjectOrder
        };
    }
}