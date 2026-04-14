using System;
using System.Collections.Generic;
using EnemPrep.EntityModels.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql;

namespace EnemPrep.EntityModels.Models;

public partial class EnemContext : DbContext
{
    public EnemContext()
    {
    }

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // modelBuilder.HasPostgresEnum<ColorScheme>
        //     ("auth", "color_scheme");
        //
        // modelBuilder.HasPostgresEnum<UserRole>
        //     ("auth", "user_role");
        //
        // modelBuilder.HasPostgresEnum<Language>
        //     ("content", "language");
        //
        // modelBuilder.HasPostgresEnum<SubjectName>
        //     ("content", "subject_name");
        //
        // modelBuilder.HasPostgresEnum<DayOfTheWeek>
        //     ("planning", "day_of_the_week");
        //
        // modelBuilder.HasPostgresEnum<ExamStatus>
        //     ("tracking", "exam_status");

        modelBuilder.Entity<Exam>(entity =>
        {
            entity.HasKey(e => e.ExamId).HasName("exam_pkey");

            entity.ToTable("exam", "tracking");
            
            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasColumnType("tracking.exam_status")
                .HasConversion<string>();
            
            entity.Property(e => e.LanguageChoice)
                .HasColumnName("language_choice")
                .HasColumnType("content.language")
                .HasConversion<string>();

            entity.Property(e => e.ExamId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("exam_id");
            entity.Property(e => e.CorrectQuestionsCount).HasColumnName("correct_questions_count");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.EstimatedScore).HasColumnName("estimated_score");
            entity.Property(e => e.ExamYear).HasColumnName("exam_year");
            entity.Property(e => e.IncorrectQuestionsCount).HasColumnName("incorrect_questions_count");
            entity.Property(e => e.MaxSpentTime).HasColumnName("max_spent_time");
            entity.Property(e => e.QuestionsCount).HasColumnName("questions_count");
            entity.Property(e => e.Title)
                .HasMaxLength(20)
                .HasColumnName("title");
            entity.Property(e => e.TotalSpentTime).HasColumnName("total_spent_time");
            entity.Property(e => e.UnsolvedQuestionsCount).HasColumnName("unsolved_questions_count");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Exams)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_user_id");
        });

        modelBuilder.Entity<ExamQuestion>(entity =>
        {
            entity.HasKey(e => new { e.ExamId, e.QuestionId }).HasName("exam_question_pk");

            entity.ToTable("exam_question", "tracking");

            entity.Property(e => e.ExamId).HasColumnName("exam_id");
            entity.Property(e => e.QuestionId).HasColumnName("question_id");
            entity.Property(e => e.ChosenAlternative)
                .HasMaxLength(1)
                .HasColumnName("chosen_alternative");
            entity.Property(e => e.CorrectAlternative)
                .HasMaxLength(1)
                .HasColumnName("correct_alternative");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.IsCorrect).HasColumnName("is_correct");
            entity.Property(e => e.TimeSpent).HasColumnName("time_spent");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Exam).WithMany(p => p.ExamQuestions)
                .HasForeignKey(d => d.ExamId)
                .HasConstraintName("exam_question_exam_id_fkey");

            entity.HasOne(d => d.Question).WithMany(p => p.ExamQuestions)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("exam_question_question_id_fkey");
        });

        modelBuilder.Entity<ExamSubject>(entity =>
        {
            entity.HasKey(e => new { e.SubjectId, e.ExamId }).HasName("pk_exam_subject");

            entity.ToTable("exam_subject", "tracking");

            entity.Property(e => e.SubjectId).HasColumnName("subject_id");
            entity.Property(e => e.ExamId).HasColumnName("exam_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Exam).WithMany(p => p.ExamSubjects)
                .HasForeignKey(d => d.ExamId)
                .HasConstraintName("exam_subject_exam_id_fkey");

            entity.HasOne(d => d.Subject).WithMany(p => p.ExamSubjects)
                .HasForeignKey(d => d.SubjectId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("exam_subject_subject_id_fkey");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.QuestionId).HasName("question_pkey");

            entity.ToTable("question", "content");

            entity.Property(e => e.Language)
                .HasColumnName("language")
                .HasColumnType("content.language")
                .HasConversion<string>();

            entity.Property(e => e.QuestionId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("question_id");
            entity.Property(e => e.ApiIndex).HasColumnName("api_index");
            entity.Property(e => e.Content)
                .HasColumnType("jsonb")
                .HasColumnName("content");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.PostedById).HasColumnName("posted_by_id");
            entity.Property(e => e.SubjectId).HasColumnName("subject_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.PostedBy).WithMany(p => p.Questions)
                .HasForeignKey(d => d.PostedById)
                .HasConstraintName("fk_user_id");

            entity.HasOne(d => d.Subject).WithMany(p => p.Questions)
                .HasForeignKey(d => d.SubjectId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_subject_id");
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("schedule_pkey");

            entity.ToTable("schedule", "planning");

            entity.Property(e => e.ScheduleId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("schedule_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Cronograma Semanal'::character varying")
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_user_id");
        });

        modelBuilder.Entity<ScheduleSubject>(entity =>
        {
            entity.HasKey(e => new { e.ScheduleId, e.SubjectId }).HasName("pk_schedule_subject");

            entity.ToTable("schedule_subject", "planning");
            
            entity.Property(e => e.Weekday)
                .HasColumnName("weekday")
                .HasColumnType("planning.day_of_the_week")
                .HasConversion<string>();
            
            entity.Property(e => e.ScheduleId).HasColumnName("schedule_id");
            entity.Property(e => e.SubjectId).HasColumnName("subject_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.SubjectOrder).HasColumnName("subject_order");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Schedule).WithMany(p => p.ScheduleSubjects)
                .HasForeignKey(d => d.ScheduleId)
                .HasConstraintName("schedule_subject_schedule_id_fkey");

            entity.HasOne(d => d.Subject).WithMany(p => p.ScheduleSubjects)
                .HasForeignKey(d => d.SubjectId)
                .HasConstraintName("schedule_subject_subject_id_fkey");
        });

        modelBuilder.Entity<SolvedQuestion>(entity =>
        {
            entity.HasKey(e => e.SolvedQuestionId).HasName("solved_question_pkey");

            entity.ToTable("solved_question", "tracking");

            entity.Property(e => e.SolvedQuestionId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("solved_question_id");
            entity.Property(e => e.ChosenAlternative)
                .HasMaxLength(1)
                .HasColumnName("chosen_alternative");
            entity.Property(e => e.CorrectAlternative)
                .HasMaxLength(1)
                .HasColumnName("correct_alternative");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.IsCorrect).HasColumnName("is_correct");
            entity.Property(e => e.QuestionId).HasColumnName("question_id");
            entity.Property(e => e.QuestionYear).HasColumnName("question_year");
            entity.Property(e => e.TimeSpent).HasColumnName("time_spent");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Question).WithMany(p => p.SolvedQuestions)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("solved_question_question_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.SolvedQuestions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("solved_question_user_id_fkey");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.SubjectId).HasName("subject_pkey");

            entity.ToTable("subject", "content");

            entity.Property(e => e.Name)
                .HasColumnName("name")
                .HasColumnType("content.subject_name");

            entity.Property(e => e.SubjectId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("subject_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("user_pkey");

            entity.ToTable("user", "auth");

            entity.Property(u => u.UserRole)
                .HasColumnName("role")
                .HasColumnType("auth.user_role")
                .HasConversion((ValueConverter?)null);
            
            entity.HasIndex(e => e.Username, "user_username_key").IsUnique();

            entity.Property(e => e.UserId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("user_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.DateOfBirth)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_of_birth");
            entity.Property(e => e.Email)
                .HasMaxLength(320)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .HasColumnName("full_name");
            entity.Property(e => e.HashPassword).HasColumnName("hash_password");
            entity.Property(e => e.IsPrivate)
                .HasDefaultValue(false)
                .HasColumnName("is_private");
            entity.Property(e => e.ProfileImage).HasColumnName("profile_image");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
            entity.Property(e => e.Username)
                .HasMaxLength(20)
                .HasColumnName("username");
        });

        modelBuilder.Entity<UserGoal>(entity =>
        {
            entity.HasKey(e => e.UserGoalId).HasName("user_goal_pkey");

            entity.ToTable("user_goal", "planning");

            entity.HasIndex(e => e.UserId, "unique_goal").IsUnique();

            entity.Property(e => e.UserGoalId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("user_goal_id");
            entity.Property(e => e.CourseName)
                .HasMaxLength(255)
                .HasColumnName("course_name");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.CutOffScore).HasColumnName("cut_off_score");
            entity.Property(e => e.UniversityName)
                .HasMaxLength(255)
                .HasColumnName("university_name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithOne(p => p.UserGoal)
                .HasForeignKey<UserGoal>(d => d.UserId)
                .HasConstraintName("fk_user_id");
        });

        modelBuilder.Entity<UserPreference>(entity =>
        {
            entity.HasKey(e => e.UserPreferencesId).HasName("user_preferences_pkey");

            entity.ToTable("user_preferences", "auth");
            
            entity.Property(u => u.ColorScheme)
                .HasColumnName("color_scheme")
                .HasColumnType("auth.color_scheme")
                .HasConversion<string>();
            
            entity.Property(u => u.ExamLanguage)
                .HasColumnName("exam_language")
                .HasColumnType("content.language")
                .HasConversion<string>();

            entity.Property(e => e.UserPreferencesId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("user_preferences_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.QuestionsPerDay)
                .HasDefaultValue(10)
                .HasColumnName("questions_per_day");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.UserPreferences)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_user_id");
        });

        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("user_profile_pkey");

            entity.ToTable("user_profile", "tracking");

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("user_id");
            entity.Property(e => e.CurrentLevel)
                .HasDefaultValue(1)
                .HasColumnName("current_level");
            entity.Property(e => e.ExperiencePoints)
                .HasDefaultValue(0)
                .HasColumnName("experience_points");
            entity.Property(e => e.LastActivityDate)
                .HasDefaultValueSql("now()")
                .HasColumnName("last_activity_date");
            entity.Property(e => e.StreakCount)
                .HasDefaultValue(0)
                .HasColumnName("streak_count");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserBio)
                .HasMaxLength(400)
                .HasColumnName("user_bio");

            entity.HasOne(d => d.User).WithOne(p => p.UserProfile)
                .HasForeignKey<UserProfile>(d => d.UserId)
                .HasConstraintName("user_profile_user_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
