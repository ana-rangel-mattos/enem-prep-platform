using EnemPrep.Domain.Enums;

namespace EnemPrep.Domain.DTOS;

public class UpdateUserPreferencesDto
{
    public int QuestionsPerDay { get; set; }
    public Language ExamLanguage { get; set; }
    public ColorScheme ColorScheme { get; set; }
}