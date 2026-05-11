using Community.Microsoft.Extensions.Caching.PostgreSql;
using EnemPrep.Domain.Enums;
using EnemPrep.Domain.Models;
using EnemPrep.Persistence;
using EnemPrep.Persistence.Constants;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using Respawn;
using Respawn.Graph;
using Testcontainers.PostgreSql;

namespace EnemPrep.Tests;

public sealed class CustomWebApplicationFactory 
    : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:17-alpine")
        .WithDatabase("enem_prep_test_db")
        .WithUsername("postgres")
        .WithPassword("password")
        .Build();

    private Respawner _respawner = null!;

    public string DbConnectionString => _dbContainer.GetConnectionString();

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        using var scope = Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<EnemContext>();
        
        await context.Database.MigrateAsync();
        
        var roles = Role.GetValues();

        if (roles.Count == 0)
        {
            throw new InvalidOperationException(
                "No roles were discovered.");
        }

        var existingRolesIds = await context.Set<Role>()
            .Select(role => role.Id)
            .ToListAsync();

        var missingRoles = roles
            .Where(role => !existingRolesIds.Contains(role.Id))
            .ToList();

        if (missingRoles.Count > 0)
        {
            context.Set<Role>().AddRange(missingRoles);
            await context.SaveChangesAsync();
        }

        await using var connection = new NpgsqlConnection(DbConnectionString);
        await connection.OpenAsync();

        _respawner = await Respawner.CreateAsync(connection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = ["public", "auth", "content", "tracking", "planning"],
            TablesToIgnore = [
                new Table("__EFMigrationsHistory"),
                new Table(SchemaNames.Auth, TableNames.Roles),
                new Table(SchemaNames.Auth, TableNames.Permissions),
                new Table(SchemaNames.Auth, TableNames.RolePermissions),
                new Table(SchemaNames.Auth, TableNames.UserRoles)
            ]
        });
    }

    public async Task ResetDatabaseAsync()
    {
        await using var connection = new NpgsqlConnection(DbConnectionString);
        await connection.OpenAsync();

        await _respawner.ResetAsync(connection);
    }

    public new async Task DisposeAsync() => await _dbContainer.StopAsync();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<DbContextOptions<EnemContext>>();
            
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(_dbContainer.GetConnectionString());

            dataSourceBuilder.MapEnum<ColorScheme>("auth.color_scheme");
            dataSourceBuilder.MapEnum<DayOfTheWeek>("planning.day_of_the_week");
            dataSourceBuilder.MapEnum<ExamStatus>("tracking.exam_status");
            dataSourceBuilder.MapEnum<Language>("content.language");
            dataSourceBuilder.MapEnum<SubjectName>("content.subject_name");

            var dataSource = dataSourceBuilder.Build();
            
            services.AddDbContext<EnemContext>(options =>
            {
                options.UseNpgsql(dataSource);
            });

            services.AddDistributedPostgreSqlCache(options =>
            {
                options.ConnectionString = _dbContainer.GetConnectionString();
                options.SchemaName = "auth";
                options.TableName = "sessions";
            });
            
            services.AddSession(options =>
            {
                options.Cookie.Name = ".EnemPrep.TestSession";
            });
        });
    }
}