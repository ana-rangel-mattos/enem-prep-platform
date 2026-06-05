using EnemPrep.Domain.DTOS;
using EnemPrep.Domain.Enums;
using EnemPrep.Domain.Result;
using EnemPrep.Persistence;
using EnemPrep.ServicesContracts;
using Microsoft.EntityFrameworkCore;

namespace EnemPrep.Services;

public class SubjectsServices : ISubjectsService
{
    private readonly EnemContext _context;

    public SubjectsServices(EnemContext context)
    {
        _context = context;
    }
    
    public async Task<ICollection<SubjectDto>> FetchSubjectsAsync(CancellationToken cancellationToken)
    {
        List<SubjectDto> subjects = await _context.Subjects
            .Select(s => new SubjectDto
            {
                SubjectId = s.SubjectId,
                Name = ConvertSubjectToString(s.Name)
            })
            .ToListAsync(cancellationToken);

        return subjects;
    }

    public async Task<Result<SubjectDto>> FetchSubjectByIdAsync(Guid? id, CancellationToken cancellationToken)
    {
        if (id is null)
        {
            return Result.Failure<SubjectDto>(SubjectErrors.SubjectNullId);
        }
        
        SubjectDto? subject = await _context.Subjects
            .Where(s => s.SubjectId == id)
            .Select(s => new SubjectDto
            {
                SubjectId = s.SubjectId,
                Name = ConvertSubjectToString(s.Name)
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (subject is null)
        {
            return Result.Failure<SubjectDto>(SubjectErrors.SubjectNotFound(id));
        }

        return Result.Success(subject);
    }

    public async Task<Result> AddSubjectAsync(SubjectName subject, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> RemoveSubjectAsync(Guid? id, CancellationToken cancellationToken)
    {
        if (id is null)
        {
            return Result.Failure(SubjectErrors.SubjectNullId);
        }

        int deletedRows = await _context.Subjects
            .Where(s => s.SubjectId == id)
            .ExecuteDeleteAsync(cancellationToken);

        if (deletedRows == 0)
        {
            return Result.Failure(SubjectErrors.SubjectNotFound(id));
        }

        await _context.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
    
    private static string ConvertSubjectToString(SubjectName subject)
    {
        return subject switch
        {
            SubjectName.Languages => "Linguagens",
            SubjectName.Humanities => "Ciências Humanas",
            SubjectName.Mathematics => "Matemática",
            SubjectName.NaturalSciences => "Ciências da Natureza",
            _ => throw new ArgumentOutOfRangeException(
                nameof(subject), 
                subject.ToString(), 
                $"{subject.ToString()} is not a valid subject name."
            )
        };
    }
}