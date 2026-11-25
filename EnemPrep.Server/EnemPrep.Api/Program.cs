using System.Reflection;
using System.Xml.Linq;
using System.Xml.XPath;
using Community.Microsoft.Extensions.Caching.PostgreSql;
using dotenv.net;
using EnemPrep.Domain.Enums;
using EnemPrep.Infrastructure.Authorization;
using EnemPrep.Persistence;
using EnemPrep.Api.Middlewares;
using EnemPrep.Application;
using EnemPrep.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Npgsql;

DotEnv.Load(options: new DotEnvOptions(ignoreExceptions: true));

var builder = WebApplication.CreateBuilder(args);

var corsOrigin = builder.Configuration["CROSS_ORIGIN_ADDRESS"]
                 ?? Environment.GetEnvironmentVariable("CROSS_ORIGIN_ADDRESS")
                 ?? "http://localhost:3000";

const string frontEndCorsPolicy = "FrontEndCorsPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(frontEndCorsPolicy, policy =>
    {
        policy.WithOrigins(corsOrigin)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddAuthorization();
builder.Services.AddHealthChecks();

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

builder.Services.AddApplication();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.NumberHandling =
        System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals;
});

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
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

        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var apiXmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
        
        var coreXmlFilename = $"EnemPrep.Application.xml";
        var coreXmlPath = Path.Combine(AppContext.BaseDirectory, coreXmlFilename);

        var xmlDocs = new List<XDocument>();
        
        if (File.Exists(apiXmlPath)) xmlDocs.Add(XDocument.Load(apiXmlPath));
        if (File.Exists(coreXmlPath)) xmlDocs.Add(XDocument.Load(coreXmlPath));
        
        var memberCache = new Dictionary<string, XElement>();
        
        foreach (var doc in xmlDocs)
        {
            var members = doc.XPathSelectElements("/doc/members/member[@name and not(inheritdoc)]");
            foreach (var member in members)
            {
                var nameAttr = member.Attribute("name")?.Value;
                if (nameAttr != null)
                {
                    memberCache[nameAttr] = member;
                }
            }
        }

        foreach (var doc in xmlDocs)
        {
            var inheritingMembers = doc.XPathSelectElements("/doc/members/member[inheritdoc]");
            foreach (var member in inheritingMembers)
            {
                var inheritdocElement = member.Element("inheritdoc");
                var cref = inheritdocElement?.Attribute("cref")?.Value;

                if (cref is not null)
                {
                    var lookupKey = cref.StartsWith("M:") ? cref : $"M:{cref}";

                    var matchedDoc = memberCache.TryGetValue(lookupKey, out var found)
                        ? found
                        : memberCache.Keys.FirstOrDefault(k => k.EndsWith(cref)) is string key
                            ? memberCache[key]
                            : null;

                    if (matchedDoc != null)
                    {
                        inheritdocElement?.Remove();
                        member.Add(matchedDoc.Nodes());
                    }
                }
            }
            
            options.IncludeXmlComments(() => new XPathDocument(doc.CreateReader()), true);
        }
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
        opts.MigrationsHistoryTable("__EFMigrationsHistory", "public");
        
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

app.MapHealthChecks("/health");

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
