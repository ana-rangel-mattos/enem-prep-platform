using EnemPrep.Domain.Enums;
using EnemPrep.Domain.Models;
using EnemPrep.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Permission = EnemPrep.Domain.Models.Permission;

namespace EnemPrep.Persistence;

public partial class EnemContext : DbContext
{
    public EnemContext()
    { }

    public EnemContext(DbContextOptions<EnemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Exam> Exams { get; set; }

    public virtual DbSet<ExamQuestion> ExamQuestions { get; set; }

    public virtual DbSet<ExamSubject> ExamSubjects { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<ScheduleSubject> ScheduleSubjects { get; set; }

    public virtual DbSet<SolvedQuestion> SolvedQuestions { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserGoal> UserGoals { get; set; }

    public virtual DbSet<UserPreference> UserPreferences { get; set; }

    public virtual DbSet<UserProfile> UserProfiles { get; set; }
    
    public virtual DbSet<Role> Roles { get; set; }
    
    public virtual DbSet<Permission> Permissions { get; set; }
    public virtual DbSet<InvitationCode> InvitationCodes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum<ColorScheme>
            (SchemaNames.Auth, "color_scheme");
        modelBuilder.HasPostgresEnum<UserRole>
            (SchemaNames.Auth, "user_role");
        modelBuilder.HasPostgresEnum<Language>
            (SchemaNames.Content, "language");
        modelBuilder.HasPostgresEnum<SubjectName>
            (SchemaNames.Content, "subject_name");
        modelBuilder.HasPostgresEnum<DayOfTheWeek>
            (SchemaNames.Planning, "day_of_the_week");
        modelBuilder.HasPostgresEnum<ExamStatus>
            (SchemaNames.Tracking, "exam_status");
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EnemContext).Assembly);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
