using EnemPrep.Domain.DTOS;
using EnemPrep.Domain.Enums;
using EnemPrep.Domain.Models;
using EnemPrep.Domain.Result;
using EnemPrep.Persistence;
using EnemPrep.ServicesContracts;
using Microsoft.EntityFrameworkCore;

namespace EnemPrep.Services;

public class UserPreferencesService : IUserPreferencesService
{
    private readonly EnemContext _context;
    private readonly IUserContext _userContext;
    
    public UserPreferencesService(EnemContext context, IUserContext userContext)
    {
        _context = context;
        _userContext = userContext;
    }
    
    public async Task<Result> ResetUserPreferencesAsync(CancellationToken cancellationToken = default)
    {
        Guid userId = _userContext.UserId;
        
        UserPreference? preference = await _context.UserPreferences
            .Where(e => e.UserId == userId)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (preference is null)
        {
            return Result.Failure(UserPreferencesErrors.UserPreferenceWasNotFound(userId));
        }
        
        UpdateUserPreferencesDto dto = new UpdateUserPreferencesDto
        {
            ColorScheme = ColorScheme.OS,
            ExamLanguage = Language.English,
            QuestionsPerDay = 5,
        };

        int updatedRows = await _context.UserPreferences
            .Where(e => e.UserId == userId)
            .ExecuteUpdateAsync(e => e
                .SetProperty(d => d.ColorScheme, dto.ColorScheme)
                .SetProperty(d => d.ExamLanguage, dto.ExamLanguage)
                .SetProperty(d => d.QuestionsPerDay, dto.QuestionsPerDay), 
                cancellationToken);

        if (updatedRows == 0)
        {
            return Result.Failure(UserPreferencesErrors.FailedToResetUserPreferences);
        }

        return Result.Success();
    }

    public async Task<Result> UpdateUserPreferencesAsync(UpdateUserPreferencesDto request, CancellationToken cancellationToken = default)
    {
        Guid userId = _userContext.UserId;
        
        UserPreference? preference = await _context.UserPreferences
            .Where(e => e.UserId == userId)
            .FirstOrDefaultAsync(cancellationToken);

        if (preference is null)
        {
            return Result.Failure(UserPreferencesErrors.UserPreferenceWasNotFound(userId));
        }

        int updatedRows = await _context.UserPreferences
            .Where(e => e.UserId == userId)
            .ExecuteUpdateAsync(e => e
                    .SetProperty(d => d.ColorScheme, request.ColorScheme)
                    .SetProperty(d => d.ExamLanguage, request.ExamLanguage)
                    .SetProperty(d => d.QuestionsPerDay, request.QuestionsPerDay), 
                cancellationToken);

        if (updatedRows == 0)
        {
            return Result.Failure(UserPreferencesErrors.FailedToUpdateUserPreferences);
        }

        return Result.Success();
    }
}