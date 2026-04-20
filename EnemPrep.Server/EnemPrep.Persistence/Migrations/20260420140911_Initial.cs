using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EnemPrep.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "tracking");

            migrationBuilder.EnsureSchema(
                name: "auth");

            migrationBuilder.EnsureSchema(
                name: "content");

            migrationBuilder.EnsureSchema(
                name: "planning");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:auth.color_scheme", "os,dark,light")
                .Annotation("Npgsql:Enum:auth.user_role", "STUDENT,ADMIN")
                .Annotation("Npgsql:Enum:content.language", "INGLES,ESPANHOL")
                .Annotation("Npgsql:Enum:content.subject_name", "linguagens,ciencias-humanas,ciencias-natureza,matematica")
                .Annotation("Npgsql:Enum:planning.day_of_the_week", "monday,tuesday,wednesday,thursday,friday,saturday,sunday")
                .Annotation("Npgsql:Enum:tracking.exam_status", "not_started,in_progress,finished,canceled");

            migrationBuilder.CreateTable(
                name: "permissions",
                schema: "auth",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                schema: "auth",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "subject",
                schema: "content",
                columns: table => new
                {
                    subject_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<int>(type: "content.subject_name", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("subject_pkey", x => x.subject_id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                schema: "auth",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    full_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    username = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    date_of_birth = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    email = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: false),
                    hash_password = table.Column<string>(type: "text", nullable: false),
                    is_private = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    profile_image = table.Column<byte[]>(type: "bytea", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_pkey", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "RolePermission",
                schema: "auth",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "integer", nullable: false),
                    PermissionId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermission", x => new { x.RoleId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_RolePermission_permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalSchema: "auth",
                        principalTable: "permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermission_roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "auth",
                        principalTable: "roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exam",
                schema: "tracking",
                columns: table => new
                {
                    exam_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    status = table.Column<int>(type: "tracking.exam_status", nullable: false),
                    language_choice = table.Column<int>(type: "content.language", nullable: false),
                    exam_year = table.Column<int>(type: "integer", nullable: false),
                    questions_count = table.Column<int>(type: "integer", nullable: false),
                    correct_questions_count = table.Column<int>(type: "integer", nullable: true),
                    incorrect_questions_count = table.Column<int>(type: "integer", nullable: true),
                    unsolved_questions_count = table.Column<int>(type: "integer", nullable: true),
                    total_spent_time = table.Column<TimeSpan>(type: "interval", nullable: true),
                    max_spent_time = table.Column<TimeSpan>(type: "interval", nullable: false),
                    estimated_score = table.Column<float>(type: "real", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("exam_pkey", x => x.exam_id);
                    table.ForeignKey(
                        name: "fk_user_id",
                        column: x => x.user_id,
                        principalSchema: "auth",
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "question",
                schema: "content",
                columns: table => new
                {
                    question_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    subject_id = table.Column<Guid>(type: "uuid", nullable: true),
                    posted_by_id = table.Column<Guid>(type: "uuid", nullable: false),
                    api_index = table.Column<int>(type: "integer", nullable: false),
                    language = table.Column<int>(type: "content.language", nullable: true),
                    content = table.Column<string>(type: "jsonb", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("question_pkey", x => x.question_id);
                    table.ForeignKey(
                        name: "fk_subject_id",
                        column: x => x.subject_id,
                        principalSchema: "content",
                        principalTable: "subject",
                        principalColumn: "subject_id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_user_id",
                        column: x => x.posted_by_id,
                        principalSchema: "auth",
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleUser",
                schema: "auth",
                columns: table => new
                {
                    RolesId = table.Column<int>(type: "integer", nullable: false),
                    UsersUserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleUser", x => new { x.RolesId, x.UsersUserId });
                    table.ForeignKey(
                        name: "FK_RoleUser_roles_RolesId",
                        column: x => x.RolesId,
                        principalSchema: "auth",
                        principalTable: "roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleUser_user_UsersUserId",
                        column: x => x.UsersUserId,
                        principalSchema: "auth",
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "schedule",
                schema: "planning",
                columns: table => new
                {
                    schedule_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValueSql: "'Cronograma Semanal'::character varying"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("schedule_pkey", x => x.schedule_id);
                    table.ForeignKey(
                        name: "fk_user_id",
                        column: x => x.user_id,
                        principalSchema: "auth",
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_goal",
                schema: "planning",
                columns: table => new
                {
                    user_goal_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    university_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    course_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    cut_off_score = table.Column<float>(type: "real", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_goal_pkey", x => x.user_goal_id);
                    table.ForeignKey(
                        name: "fk_user_id",
                        column: x => x.user_id,
                        principalSchema: "auth",
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_preferences",
                schema: "auth",
                columns: table => new
                {
                    user_preferences_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    questions_per_day = table.Column<int>(type: "integer", nullable: false, defaultValue: 10),
                    exam_language = table.Column<string>(type: "content.language", nullable: false),
                    color_scheme = table.Column<string>(type: "auth.color_scheme", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_preferences_pkey", x => x.user_preferences_id);
                    table.ForeignKey(
                        name: "fk_user_id",
                        column: x => x.user_id,
                        principalSchema: "auth",
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_profile",
                schema: "tracking",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_bio = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: true),
                    streak_count = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    experience_points = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    current_level = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    last_activity_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_profile_pkey", x => x.user_id);
                    table.ForeignKey(
                        name: "user_profile_user_id_fkey",
                        column: x => x.user_id,
                        principalSchema: "auth",
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exam_subject",
                schema: "tracking",
                columns: table => new
                {
                    subject_id = table.Column<Guid>(type: "uuid", nullable: false),
                    exam_id = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_exam_subject", x => new { x.subject_id, x.exam_id });
                    table.ForeignKey(
                        name: "exam_subject_exam_id_fkey",
                        column: x => x.exam_id,
                        principalSchema: "tracking",
                        principalTable: "exam",
                        principalColumn: "exam_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "exam_subject_subject_id_fkey",
                        column: x => x.subject_id,
                        principalSchema: "content",
                        principalTable: "subject",
                        principalColumn: "subject_id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "exam_question",
                schema: "tracking",
                columns: table => new
                {
                    exam_id = table.Column<Guid>(type: "uuid", nullable: false),
                    question_id = table.Column<Guid>(type: "uuid", nullable: false),
                    correct_alternative = table.Column<char>(type: "character(1)", maxLength: 1, nullable: false),
                    chosen_alternative = table.Column<char>(type: "character(1)", maxLength: 1, nullable: true),
                    is_correct = table.Column<bool>(type: "boolean", nullable: false),
                    time_spent = table.Column<TimeSpan>(type: "interval", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("exam_question_pk", x => new { x.exam_id, x.question_id });
                    table.ForeignKey(
                        name: "exam_question_exam_id_fkey",
                        column: x => x.exam_id,
                        principalSchema: "tracking",
                        principalTable: "exam",
                        principalColumn: "exam_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "exam_question_question_id_fkey",
                        column: x => x.question_id,
                        principalSchema: "content",
                        principalTable: "question",
                        principalColumn: "question_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "solved_question",
                schema: "tracking",
                columns: table => new
                {
                    solved_question_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    question_id = table.Column<Guid>(type: "uuid", nullable: false),
                    question_year = table.Column<int>(type: "integer", nullable: false),
                    correct_alternative = table.Column<char>(type: "character(1)", maxLength: 1, nullable: false),
                    chosen_alternative = table.Column<char>(type: "character(1)", maxLength: 1, nullable: true),
                    is_correct = table.Column<bool>(type: "boolean", nullable: false),
                    time_spent = table.Column<TimeSpan>(type: "interval", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("solved_question_pkey", x => x.solved_question_id);
                    table.ForeignKey(
                        name: "solved_question_question_id_fkey",
                        column: x => x.question_id,
                        principalSchema: "content",
                        principalTable: "question",
                        principalColumn: "question_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "solved_question_user_id_fkey",
                        column: x => x.user_id,
                        principalSchema: "auth",
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "schedule_subject",
                schema: "planning",
                columns: table => new
                {
                    schedule_id = table.Column<Guid>(type: "uuid", nullable: false),
                    subject_id = table.Column<Guid>(type: "uuid", nullable: false),
                    weekday = table.Column<int>(type: "planning.day_of_the_week", nullable: false),
                    subject_order = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_schedule_subject", x => new { x.schedule_id, x.subject_id });
                    table.ForeignKey(
                        name: "schedule_subject_schedule_id_fkey",
                        column: x => x.schedule_id,
                        principalSchema: "planning",
                        principalTable: "schedule",
                        principalColumn: "schedule_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "schedule_subject_subject_id_fkey",
                        column: x => x.subject_id,
                        principalSchema: "content",
                        principalTable: "subject",
                        principalColumn: "subject_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "auth",
                table: "permissions",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "CreateQuestions" },
                    { 2, "ReadContent" },
                    { 3, "DeleteUser" },
                    { 4, "DeleteQuestion" },
                    { 5, "EditQuestion" }
                });

            migrationBuilder.InsertData(
                schema: "auth",
                table: "roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Student" },
                    { 2, "Admin" }
                });

            migrationBuilder.InsertData(
                schema: "auth",
                table: "RolePermission",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { 2, 1 },
                    { 1, 2 },
                    { 3, 2 },
                    { 4, 2 },
                    { 5, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_exam_user_id",
                schema: "tracking",
                table: "exam",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_exam_question_question_id",
                schema: "tracking",
                table: "exam_question",
                column: "question_id");

            migrationBuilder.CreateIndex(
                name: "IX_exam_subject_exam_id",
                schema: "tracking",
                table: "exam_subject",
                column: "exam_id");

            migrationBuilder.CreateIndex(
                name: "IX_question_posted_by_id",
                schema: "content",
                table: "question",
                column: "posted_by_id");

            migrationBuilder.CreateIndex(
                name: "IX_question_subject_id",
                schema: "content",
                table: "question",
                column: "subject_id");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_PermissionId",
                schema: "auth",
                table: "RolePermission",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleUser_UsersUserId",
                schema: "auth",
                table: "RoleUser",
                column: "UsersUserId");

            migrationBuilder.CreateIndex(
                name: "IX_schedule_user_id",
                schema: "planning",
                table: "schedule",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_schedule_subject_subject_id",
                schema: "planning",
                table: "schedule_subject",
                column: "subject_id");

            migrationBuilder.CreateIndex(
                name: "IX_solved_question_question_id",
                schema: "tracking",
                table: "solved_question",
                column: "question_id");

            migrationBuilder.CreateIndex(
                name: "IX_solved_question_user_id",
                schema: "tracking",
                table: "solved_question",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "user_username_key",
                schema: "auth",
                table: "user",
                column: "username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "unique_goal",
                schema: "planning",
                table: "user_goal",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_preferences_user_id",
                schema: "auth",
                table: "user_preferences",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "exam_question",
                schema: "tracking");

            migrationBuilder.DropTable(
                name: "exam_subject",
                schema: "tracking");

            migrationBuilder.DropTable(
                name: "RolePermission",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "RoleUser",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "schedule_subject",
                schema: "planning");

            migrationBuilder.DropTable(
                name: "solved_question",
                schema: "tracking");

            migrationBuilder.DropTable(
                name: "user_goal",
                schema: "planning");

            migrationBuilder.DropTable(
                name: "user_preferences",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "user_profile",
                schema: "tracking");

            migrationBuilder.DropTable(
                name: "exam",
                schema: "tracking");

            migrationBuilder.DropTable(
                name: "permissions",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "roles",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "schedule",
                schema: "planning");

            migrationBuilder.DropTable(
                name: "question",
                schema: "content");

            migrationBuilder.DropTable(
                name: "subject",
                schema: "content");

            migrationBuilder.DropTable(
                name: "user",
                schema: "auth");
        }
    }
}
