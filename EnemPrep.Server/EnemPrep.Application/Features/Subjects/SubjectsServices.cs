using EnemPrep.Domain.Enums;
using EnemPrep.Domain.Models;
using EnemPrep.Domain.Result;
using EnemPrep.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EnemPrep.Application.Features.Subjects;

public class SubjectsServices : ISubjectsService
{
    private readonly EnemContext _context;

    public SubjectsServices(EnemContext context)
    {
        _context = context;
    }
    
    public async Task<ICollection<GetSubjectDto>> FetchSubjectsAsync(CancellationToken cancellationToken)
    {
        var subjects = await _context.Subjects
            .Select(e => e.ToGetSubjectDto())
            .ToListAsync(cancellationToken);

        return subjects;
    }

    public async Task<Result<GetSubjectDto>> FetchSubjectByIdAsync(Guid subjectId, CancellationToken cancellationToken)
    {
        var subject = await _context.Subjects
            .SingleOrDefaultAsync(e => e.SubjectId == subjectId, cancellationToken);

        if (subject is null)
        {
            return Result.Failure<GetSubjectDto>(SubjectErrors.SubjectNotFound(subjectId));
        }

        return Result.Success(subject.ToGetSubjectDto());
    }

    public async Task<Result> CreateSubjectAsync(PostSubjectDto request, CancellationToken cancellationToken)
    {
        var newSubject = new Subject
        {
            Name = request.SubjectName
        };

        _context.Subjects.Add(newSubject);
        
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> DeleteSubjectByIdAsync(Guid subjectId, CancellationToken cancellationToken)
    {
        var subject = await _context.Subjects
            .SingleOrDefaultAsync(s => s.SubjectId == subjectId, cancellationToken);

        if (subject is null)
        {
            return Result.Failure(SubjectErrors.SubjectNotFound(subjectId));
        }

        _context.Subjects.Remove(subject);

        await _context.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}