using EnemPrep.Domain.Enums;
using EnemPrep.Domain.Models;

namespace EnemPrep.Domain.DTOS;

public class UpdateUserPreferencesDto
{
    public int QuestionsPerDay { get; set; }
    public Language ExamLanguage { get; set; }
    public ColorScheme ColorScheme { get; set; }

    public UserPreference ConvertToUserPreference()
    {
        return new UserPreference
        {
            QuestionsPerDay = QuestionsPerDay,
            ExamLanguage = ExamLanguage,
            ColorScheme = ColorScheme,
        };
    }
}