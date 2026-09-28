using EnemPrep.Domain.DTOS;
using EnemPrep.Domain.Result;

namespace EnemPrep.ServicesContracts;

public interface IScheduleService
{
    Task<Result> PostScheduleAsync(PostScheduleDto request, CancellationToken cancellationToken = default);
    Task<Result> DeleteScheduleAsync(Guid scheduleId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<GetScheduleDto>> FetchAllSchedulesAsync(CancellationToken cancellationToken = default);
    Task<Result<GetScheduleDetailsDto>> FetchScheduleByIdAsync(Guid scheduleId, CancellationToken cancellationToken = default);
    Task<Result> UpdateScheduleOrderAsync(Guid scheduleId, UpdateScheduleOrderDto request,
        CancellationToken cancellationToken = default);

    Task<Result> UpdateScheduleSubjectDayAsync(Guid scheduleId, UpdateScheduleSubjectWeekDayDto request,
        CancellationToken cancellationToken = default);
}