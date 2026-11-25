using EnemPrep.Application.Common.Models;
using EnemPrep.Domain.Result;

namespace EnemPrep.Application.Features.Exams;

public interface IExamsService
{
    Task<Result<GetExamDto>> GenerateExamAsync(PostExamDto request, CancellationToken cancellationToken = default);
    Task<Result> UpdateExamStatus(UpdateExamStatusDto request, CancellationToken cancellationToken = default);
    Task<Result<TakeExamDto>> TakeExamAsync(Guid examId, CancellationToken cancellationToken = default);
    Task<Result> SubmitAnswerAsync(SubmitAnswerRequest request, CancellationToken cancellationToken = default);
    Task<Result<GetExamDto>> SubmitExam(Guid examId, CancellationToken cancellationToken = default);
    Task<PagedResponse<GetExamDto>> FetchAllExams(ExamQueryFilter filter, CancellationToken cancellationToken = default);
}