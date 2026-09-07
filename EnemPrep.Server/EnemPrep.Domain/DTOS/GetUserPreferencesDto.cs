using EnemPrep.Domain.Enums;

namespace EnemPrep.Domain.DTOS;

public class GetUserPreferencesDto
{
    public Guid UserPreferencesId { get; set; }
    public Guid UserId { get; set; }
    public int QuestionsPerDay { get; set; }
    public Language ExamLanguage { get; set; }
    public ColorScheme ColorScheme { get; set; }
}