using Community.Microsoft.Extensions.Caching.PostgreSql;
using EnemPrep.Domain.Enums;
using EnemPrep.Infrastructure.Authorization;
using EnemPrep.Persistence;
using EnemPrep.Services;
using EnemPrep.ServicesContracts;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

const string frontEndCorsPolicy = "FrontEndCorsPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(frontEndCorsPolicy, policy =>
    {
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

builder.Services.AddAuthorization();

builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();

builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IQuestionService, QuestionsService>();
builder.Services.AddControllers();

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
    });
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
         options.LoginPath = "/api/auth/login";
         options.LogoutPath = "/api/auth/logout";
    });

builder.Services.AddHttpContextAccessor();

var app = builder.Build();
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

app.UseStaticFiles();
app.UseRouting();
app.UseCors(frontEndCorsPolicy);
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program {}