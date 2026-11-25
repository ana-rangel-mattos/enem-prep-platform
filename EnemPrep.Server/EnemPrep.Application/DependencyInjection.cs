using EnemPrep.Application.Common.Interfaces;
using EnemPrep.Application.Features.Articles;
using EnemPrep.Application.Features.Auth;
using EnemPrep.Application.Features.Exams;
using EnemPrep.Application.Features.Playlists;
using EnemPrep.Application.Features.Questions;
using EnemPrep.Application.Features.Schedules;
using EnemPrep.Application.Features.SolvedQuestions;
using EnemPrep.Application.Features.Subjects;
using EnemPrep.Application.Features.UserGoals;
using EnemPrep.Application.Features.UserPreferences;
using EnemPrep.Application.Features.UserProfiles;
using EnemPrep.Application.Features.Users;
using EnemPrep.ServicesContracts;
using Microsoft.Extensions.DependencyInjection;

namespace EnemPrep.Application;

/// <summary>
/// <see cref="IServiceCollection"/> extension class.
/// </summary>
public static class ApplicationServiceExtensions
{
    /// <summary>
    /// Injects the Application layer's Services
    /// </summary>
    /// <param name="services">Service Collection from <see cref="IServiceCollection"/>.</param>
    /// <returns>The same collection of services to allow chaining calls (FluentAPI).</returns>
    /// <remarks>
    /// This method injects the following components:
    /// <list type="bullet">
    /// <item><description><b>Services:</b> Application Services (e.g. <c>IUserService</c>, <c>IQuestionsService</c>).</description></item>
    /// </list>
    /// </remarks>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddTransient<IAuthService, AuthService>();
        services.AddTransient<IQuestionService, QuestionsService>();
        services.AddTransient<ISubjectsService, SubjectsServices>();
        services.AddTransient<ISolvedQuestionsService, SolvedQuestionsService>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IUserPreferencesService, UserPreferencesService>();
        services.AddTransient<IUserProfileService, UserProfileService>();
        services.AddTransient<IUserGoalService, UserGoalService>();
        services.AddTransient<IExamsService, ExamsService>();
        services.AddTransient<IScheduleService, ScheduleService>();
        services.AddTransient<IArticleService, ArticleService>();
        services.AddTransient<IPlaylistService, PlaylistService>();

        return services;
    }
}