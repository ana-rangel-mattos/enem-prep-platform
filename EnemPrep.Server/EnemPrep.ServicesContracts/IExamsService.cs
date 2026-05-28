using EnemPrep.Domain.Result;

namespace EnemPrep.ServicesContracts;

public interface IExamsService
{
    Task<Result> CreateExam(CancellationToken cancellationToken = default);
}