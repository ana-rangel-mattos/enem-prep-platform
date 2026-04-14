using Community.Microsoft.Extensions.Caching.PostgreSql;
using EnemPrep.EntityModels.Enums;
using EnemPrep.EntityModels.Models;
using EnemPrep.Services;
using EnemPrep.ServicesContracts;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Npgsql;

AppContext.SetSwitch("Npgsql.EnableStoredProcedureCompatMode", true);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddTransient<IAuthService, AuthService>();

builder.Services.AddControllers();

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);

dataSourceBuilder.MapEnum<UserRole>("auth.user_role");
dataSourceBuilder.MapEnum<ColorScheme>("auth.color_scheme");
dataSourceBuilder.MapEnum<DayOfTheWeek>("planning.day_of_the_week");
dataSourceBuilder.MapEnum<ExamStatus>("tracking.exam_status");
dataSourceBuilder.MapEnum<Language>("content.language");
dataSourceBuilder.MapEnum<SubjectName>("content.subject_name");

var dataSource = dataSourceBuilder.Build();

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
        opts.MigrationsAssembly("EnemPrep.EntityModels");
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

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllers();

app.Run();