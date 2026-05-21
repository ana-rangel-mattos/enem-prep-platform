using EnemPrep.Domain.Common;

namespace EnemPrep.Domain.DTOS;

public class FetchSolvedQuestionsDto
{
    public Guid? UserId { get; set; }
    public SolvedQuestionFilter Filter { get; set; } = new();
}