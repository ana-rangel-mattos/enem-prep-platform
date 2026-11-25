namespace EnemPrep.Domain.Result;

public static class ScheduleErrors
{
    public static Error ScheduleWasNotFound(Guid scheduleId) => new Error(
        ErrorNames.ScheduleScheduleWasNotFound,
            $"Could not find schedule with ID {scheduleId}."
        );
    
    public static Error ScheduleSubjectWasNotFound(Guid subjectId) => new Error(
        ErrorNames.ScheduleScheduleSubjectWasNotFound,
        $"Could not find subject with ID {subjectId}."
    );

    public static readonly Error ScheduleSubjectDeleteAccessForbidden = new Error(
        ErrorNames.ScheduleSubjectDeleteAccessForbidden,
        "You are not allowed to delete this schedule subject.");
}