using EnemPrep.Domain.Enums;
using EnemPrep.Domain.Models;

namespace EnemPrep.Application.Features.UserPreferences;

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

public class GetUserPreferencesDto
{
    public Guid UserPreferencesId { get; set; }
    public Guid UserId { get; set; }
    public int QuestionsPerDay { get; set; }
    public Language ExamLanguage { get; set; }
    public ColorScheme ColorScheme { get; set; }
}