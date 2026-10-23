using System.Collections.Immutable;
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

    public async Task<Result> PostScheduleSubjectAsync(Guid scheduleId, PostScheduleSubjectDto request,
        CancellationToken cancellationToken = default)
    {
        Guid loggedUserId = _userContext.UserId;

        var schedule = await _context.Schedules
            .Include(s => s.ScheduleSubjects)
            .Where(s => s.UserId == loggedUserId && s.ScheduleId == scheduleId)
            .SingleOrDefaultAsync(cancellationToken);

        if (schedule is null)
        {
            return Result.Failure(ScheduleErrors.ScheduleWasNotFound(scheduleId));
        }

        var scheduleDayLastItem = schedule.ScheduleSubjects
            .Where(s => s.Weekday == request.WeekDay)
            .OrderBy(s => s.SubjectOrder)
            .ToList().LastOrDefault();

        var newDay = request.WeekDay;
        var subject = await _context.Subjects
            .SingleOrDefaultAsync(s => s.SubjectId == request.SubjectId, cancellationToken);

        if (subject is null)
        {
            return Result.Failure(ScheduleErrors.ScheduleSubjectWasNotFound(request.SubjectId));
        }
        
        schedule.ScheduleSubjects.Add(new ScheduleSubject
        {
            SubjectId = request.SubjectId,
            ScheduleId = scheduleId,
            Weekday = newDay,
            SubjectOrder = scheduleDayLastItem?.SubjectOrder ?? 0
        });

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

    public async Task<Result> DeleteScheduleSubjectAsync(Guid scheduleId, Guid subjectId, CancellationToken cancellationToken = default)
    {
        Guid loggedUserId = _userContext.UserId;

        int deletedRows = await _context.ScheduleSubjects
            .Include(s => s.Schedule)
            .Where(s => s.Schedule.UserId == loggedUserId && s.SubjectId == subjectId && s.ScheduleId == scheduleId)
            .ExecuteDeleteAsync(cancellationToken);

        if (deletedRows > 0)
        {
            return Result.Failure(ScheduleErrors.ScheduleSubjectWasNotFound(scheduleId));
        }

        return Result.Success();
    }

    public async Task<IReadOnlyCollection<GetScheduleDto>> FetchAllSchedulesAsync(CancellationToken cancellationToken = default)
    {
        Guid loggedUserId = _userContext.UserId;
        
        var schedules = await _context.Schedules
            .Where(s => s.UserId == loggedUserId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

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

    public async Task<Result> UpdateScheduleSubjectOrderAsync(Guid scheduleId, UpdateScheduleSubjectOrderDto request,
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

        var itemToMove = schedule.ScheduleSubjects
            .SingleOrDefault(s => s.SubjectId == request.SubjectId);
        
        if (itemToMove is null)
        {
            return Result.Failure(ScheduleErrors.ScheduleSubjectWasNotFound(scheduleId));
        }

        var oldDay = itemToMove.Weekday;
        var oldIndex = itemToMove.SubjectOrder;
        var newDay = request.NewDay;
        var newIndex = request.NewIndex;

        if (oldDay == newDay && oldIndex == newIndex)
        {
            return Result.Success();
        }

        if (oldDay == newDay)
        {
            if (oldIndex < newIndex)
            {
                var itemsToShift = schedule.ScheduleSubjects
                    .Where(s => 
                        s.Weekday == oldDay 
                        && s.SubjectOrder > oldIndex 
                        && s.SubjectOrder <= newIndex);

                foreach (var item in itemsToShift)
                {
                    item.SubjectOrder--;
                }
            }
            else
            {
                var itemsToShift = schedule.ScheduleSubjects
                    .Where(s => 
                        s.Weekday == oldDay 
                        && s.SubjectOrder >= newIndex 
                        && s.SubjectOrder < oldIndex);

                foreach (var item in itemsToShift)
                {
                    item.SubjectOrder++;
                }
            }
        }
        else
        {
            var sourceDayItems = schedule.ScheduleSubjects
                .Where(s => s.Weekday == oldDay && s.SubjectOrder > oldIndex);

            foreach (var item in sourceDayItems)
            {
                item.SubjectOrder--;
            }

            var targetDayItems = schedule.ScheduleSubjects
                .Where(s => s.Weekday == newDay && s.SubjectOrder >= newIndex);

            foreach (var item in targetDayItems)
            {
                item.SubjectOrder++;
            }
        }

        itemToMove.Weekday = newDay;
        itemToMove.SubjectOrder = newIndex;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> UpdateScheduleByIdAsync(Guid scheduleId, UpdateScheduleDto request, CancellationToken cancellationToken = default)
    {
        Guid userId = _userContext.UserId;

        var schedule = await _context.Schedules
            .SingleOrDefaultAsync(e => 
                e.ScheduleId == scheduleId && 
                e.UserId == userId, cancellationToken);

        if (schedule is null)
        {
            return Result.Failure(ScheduleErrors.ScheduleWasNotFound(scheduleId));
        }

        schedule.HasWeekEnd = request.HasWeekEnd;
        schedule.Name = request.Name;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}