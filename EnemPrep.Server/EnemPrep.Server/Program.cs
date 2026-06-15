using Community.Microsoft.Extensions.Caching.PostgreSql;
using EnemPrep.Domain.Enums;
using EnemPrep.Infrastructure.Authorization;
using EnemPrep.Persistence;
using EnemPrep.Server.Middlewares;
using EnemPrep.Services;
using EnemPrep.ServicesContracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

const string frontEndCorsPolicy = "FrontEndCorsPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(frontEndCorsPolicy, policy =>
    {
        policy.WithOrigins(
                "http://192.168.1.241:3000",
                "http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddAuthorization();

builder.Services.AddTransient<GlobalExceptionMiddleware>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<IUserContext, UserContext>();
builder.Services.AddScoped<IPermissionService, PermissionService>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = SessionAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = SessionAuthenticationDefaults.AuthenticationScheme;
        options.DefaultForbidScheme = SessionAuthenticationDefaults.AuthenticationScheme;
    })
    .AddScheme<AuthenticationSchemeOptions, SessionAuthenticationHandler>(SessionAuthenticationDefaults.AuthenticationScheme, options => {});

builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IQuestionService, QuestionsService>();
builder.Services.AddTransient<ISubjectsService, SubjectsServices>();
builder.Services.AddTransient<ISolvedQuestionsService, SolvedQuestionsService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IUserPreferencesService, UserPreferencesService>();
builder.Services.AddTransient<IUserGoalService, UserGoalService>();
builder.Services.AddTransient<IExamsService, ExamsService>();
builder.Services.AddControllers();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("EnemPrep API V1", new OpenApiInfo
        {
            Version = "V1",
            Title = "EnemPrep API",
            Description = "API for EnemPrep application.",
            Contact = new OpenApiContact
            {
                Name = "Ana",
                Email = "anabrmattos@gmail.com"
            },
            License = new OpenApiLicense
            {
                Name = "GNU General Public License v3.0",
                Url = new Uri("https://github.com/ana-rangel-mattos/enem-prep-platform/blob/main/LICENSE")
            }
        });
    });
}

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);

dataSourceBuilder.MapEnum<ColorScheme>("auth.color_scheme");
dataSourceBuilder.MapEnum<DayOfTheWeek>("planning.day_of_the_week");
dataSourceBuilder.MapEnum<ExamStatus>("tracking.exam_status");
dataSourceBuilder.MapEnum<Language>("content.language");
dataSourceBuilder.MapEnum<SubjectName>("content.subject_name");

var dataSource = dataSourceBuilder.Build();
builder.Services.AddSingleton(dataSource);

builder.Services.AddDistributedPostgreSqlCache(options =>
{
    options.DataSourceFactory = () => dataSource;
    options.SchemaName = "auth";
    options.TableName = "sessions";
    options.ExpiredItemsDeletionInterval = TimeSpan.FromMinutes(30);
});

builder.Services.AddDbContext<EnemContext>(options =>
{
    options.UseNpgsql(dataSource, opts =>
    {
        opts.MigrationsAssembly("EnemPrep.Persistence");
        opts.MigrationsHistoryTable("__EFMigrationsHistory", "auth");
        
        opts.MapEnum<ColorScheme>("auth.color_scheme");
        opts.MapEnum<DayOfTheWeek>("planning.day_of_the_week");
        opts.MapEnum<ExamStatus>("tracking.exam_status");
        opts.MapEnum<Language>("content.language");
        opts.MapEnum<SubjectName>("content.subject_name");
    });
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    // options.Cookie.SameSite = SameSiteMode.None;
    // options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "EnemPrep API V1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseStaticFiles();
app.UseRouting();
app.UseCors(frontEndCorsPolicy);
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program {}