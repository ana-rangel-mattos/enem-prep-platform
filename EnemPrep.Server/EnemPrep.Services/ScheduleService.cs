using System.Collections.Immutable;
using EnemPrep.Domain.Common;
using EnemPrep.Domain.DTOS;
using EnemPrep.Domain.Extensions;
using EnemPrep.Domain.Models;
using EnemPrep.Domain.Result;
using EnemPrep.Persistence;
using EnemPrep.ServicesContracts;
using Microsoft.EntityFrameworkCore;

namespace EnemPrep.Services;

public class ScheduleService : IScheduleService
{
    private readonly EnemContext _context;
    private readonly IUserContext _userContext;
    
    public ScheduleService(EnemContext context, IUserContext userContext)
    {
        _context = context;
        _userContext = userContext;
    }
    
    public async Task<Result> PostScheduleAsync(PostScheduleDto request, CancellationToken cancellationToken = default)
    {
        Guid loggedUserId = _userContext.UserId;

        Schedule newSchedule = new Schedule
        {
            UserId = loggedUserId,
            Name = request.Name,
            HasWeekEnd = request.HasWeekEnd,
            ScheduleSubjects = new List<ScheduleSubject>()
        };

        _context.Schedules.Add(newSchedule);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> DeleteScheduleAsync(Guid scheduleId, CancellationToken cancellationToken = default)
    {
        Guid loggedUserId = _userContext.UserId;

        int deletedRows = await _context.Schedules.Where(s => s.UserId == loggedUserId && s.ScheduleId == scheduleId).ExecuteDeleteAsync(cancellationToken);

        if (deletedRows > 0)
        {
            return Result.Failure(ScheduleErrors.ScheduleWasNotFound(scheduleId));
        }

        return Result.Success();
    }

    public async Task<IReadOnlyCollection<GetScheduleDto>> FetchAllSchedulesAsync(CancellationToken cancellationToken = default)
    {
        Guid loggedUserId = _userContext.UserId;
        
        var schedules = await _context.Schedules
            .Where(s => s.UserId == loggedUserId)
            .ToListAsync<Schedule>(cancellationToken);

        return schedules.Select(s => s.ToGetScheduleDto()).ToImmutableList();
    }

    public async Task<Result<GetScheduleDetailsDto>> FetchScheduleByIdAsync(Guid scheduleId, CancellationToken cancellationToken = default)
    {
        Guid loggedUserId = _userContext.UserId;

        var schedule = await _context.Schedules
            .Include(s => s.ScheduleSubjects)
            .SingleOrDefaultAsync(
                s => s.UserId == loggedUserId && s.ScheduleId == scheduleId, 
                cancellationToken);

        if (schedule is null)
        {
            return Result.Failure<GetScheduleDetailsDto>(ScheduleErrors.ScheduleWasNotFound(scheduleId));
        }

        return Result.Success(schedule.ToGetScheduleDetailsDto());
    }

    public async Task<Result> UpdateScheduleOrderAsync(Guid scheduleId, UpdateScheduleOrderDto request,
        CancellationToken cancellationToken = default)
    {
        Guid loggedUserId = _userContext.UserId;

        var schedule = await _context.Schedules
            .Include(s => s.ScheduleSubjects)
            .SingleOrDefaultAsync(
                s => s.UserId == loggedUserId && s.ScheduleId == scheduleId,
                cancellationToken);

        if (schedule is null)
        {
            return Result.Failure(ScheduleErrors.ScheduleWasNotFound(scheduleId));
        }
        
        var subjectsToRemove = schedule.ScheduleSubjects
            .Where(ss => ss.Weekday == request.DayOfWeek)
            .ToList();

        foreach (var scheduleSubject in subjectsToRemove)
        {
            schedule.ScheduleSubjects.Remove(scheduleSubject);
        }
        
        foreach (var subjectDto in request.Items)
        {
            ScheduleSubject newItem = new ScheduleSubject
            {
                SubjectOrder = subjectDto.OrderIndex,
                Weekday = request.DayOfWeek,
                ScheduleId = subjectDto.ScheduleId,
                SubjectId = subjectDto.SubjectId
            };
            schedule.ScheduleSubjects.Add(newItem);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> UpdateScheduleSubjectDayAsync(Guid scheduleId, UpdateScheduleSubjectWeekDayDto request,
        CancellationToken cancellationToken = default)
    {
        Guid loggedUserId = _userContext.UserId;
        
        var schedule = await _context.Schedules
            .Include(s => s.ScheduleSubjects)
            .SingleOrDefaultAsync(
                s => s.UserId == loggedUserId && s.ScheduleId == scheduleId, 
                cancellationToken);
        
        if (schedule is null)
        {
            return Result.Failure(ScheduleErrors.ScheduleWasNotFound(scheduleId));
        }

        var subjectToRemove = schedule.ScheduleSubjects
            .SingleOrDefault(s => s.SubjectId == request.SubjectId);
        
        if (subjectToRemove is null)
        {
            return Result.Failure(ScheduleErrors.ScheduleSubjectWasNotFound(scheduleId));
        }

        schedule.ScheduleSubjects.Remove(subjectToRemove);
    }
}