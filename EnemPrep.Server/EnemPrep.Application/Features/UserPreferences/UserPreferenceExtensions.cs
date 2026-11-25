using EnemPrep.Domain.Models;

namespace EnemPrep.Application.Features.UserPreferences;

public static class UserPreferenceExtensions
{
    public static GetUserPreferencesDto ToGetUserPreferencesDto(this UserPreference preference)
    {
        return new GetUserPreferencesDto
        {
            UserPreferencesId = preference.UserPreferencesId,
            UserId = preference.UserId,
            QuestionsPerDay = preference.QuestionsPerDay,
            ExamLanguage = preference.ExamLanguage,
            ColorScheme = preference.ColorScheme
        };
    }
}