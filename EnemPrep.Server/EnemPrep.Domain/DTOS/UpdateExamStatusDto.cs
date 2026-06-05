using EnemPrep.Domain.Enums;

namespace EnemPrep.Domain.DTOS;

public class UpdateExamStatusDto
{
    public Guid? ExamId { get; set; }
    public ExamStatus NewStatus { get; set; }
}