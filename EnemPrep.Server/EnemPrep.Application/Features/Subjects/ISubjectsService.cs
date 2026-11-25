using EnemPrep.Domain.Result;

namespace EnemPrep.Application.Features.Subjects;

public interface ISubjectsService
{
    Task<ICollection<GetSubjectDto>> FetchSubjectsAsync(CancellationToken cancellationToken = default);
    Task<Result<GetSubjectDto>> FetchSubjectByIdAsync(Guid subjectId, CancellationToken cancellationToken = default);
    Task<Result> CreateSubjectAsync(PostSubjectDto request, CancellationToken cancellationToken = default);
    Task<Result> DeleteSubjectByIdAsync(Guid subjectId, CancellationToken cancellationToken = default);
}