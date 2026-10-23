using EnemPrep.Domain.DTOS;
using EnemPrep.Domain.Result;

namespace EnemPrep.ServicesContracts;

public interface IScheduleService
{
    Task<Result> PostScheduleAsync(PostScheduleDto request, CancellationToken cancellationToken = default);
    Task<Result> PostScheduleSubjectAsync(Guid scheduleId, PostScheduleSubjectDto request, CancellationToken cancellationToken = default);
    Task<Result> DeleteScheduleAsync(Guid scheduleId, CancellationToken cancellationToken = default);

    Task<Result> DeleteScheduleSubjectAsync(Guid scheduleId, Guid subjectId,
        CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<GetScheduleDto>> FetchAllSchedulesAsync(CancellationToken cancellationToken = default);
    Task<Result<GetScheduleDetailsDto>> FetchScheduleByIdAsync(Guid scheduleId, CancellationToken cancellationToken = default);

    Task<Result> UpdateScheduleSubjectOrderAsync(Guid scheduleId, UpdateScheduleSubjectOrderDto request,
        CancellationToken cancellationToken = default);

    Task<Result> UpdateScheduleByIdAsync(Guid scheduleId, UpdateScheduleDto request, CancellationToken cancellationToken = default);
}