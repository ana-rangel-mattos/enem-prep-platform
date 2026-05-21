using EnemPrep.Domain.DTOS;
using EnemPrep.Domain.Enums;
using EnemPrep.Domain.Result;

namespace EnemPrep.ServicesContracts;

public interface ISubjectsService
{
    Task<ICollection<SubjectDto>> FetchSubjectsAsync(CancellationToken cancellationToken = default);
    Task<Result<SubjectDto>> FetchSubjectByIdAsync(Guid? id, CancellationToken cancellationToken = default);
    Task<Result> AddSubjectAsync(SubjectName subject, CancellationToken cancellationToken = default);
    Task<Result> RemoveSubjectAsync(Guid? id, CancellationToken cancellationToken = default);
}