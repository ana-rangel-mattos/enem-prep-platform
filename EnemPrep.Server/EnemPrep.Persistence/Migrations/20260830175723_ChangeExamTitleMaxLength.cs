using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnemPrep.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeExamTitleMaxLength : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:auth.color_scheme", "dark,light,os")
                .Annotation("Npgsql:Enum:content.language", "ESPANHOL,INGLES")
                .Annotation("Npgsql:Enum:content.subject_name", "ciencias-humanas,ciencias-natureza,linguagens,matematica")
                .Annotation("Npgsql:Enum:planning.day_of_the_week", "friday,monday,saturday,sunday,thursday,tuesday,wednesday")
                .Annotation("Npgsql:Enum:tracking.exam_status", "canceled,finished,in_progress,not_started")
                .OldAnnotation("Npgsql:Enum:auth.color_scheme", "os,dark,light")
                .OldAnnotation("Npgsql:Enum:content.language", "INGLES,ESPANHOL")
                .OldAnnotation("Npgsql:Enum:content.subject_name", "linguagens,ciencias-humanas,ciencias-natureza,matematica")
                .OldAnnotation("Npgsql:Enum:planning.day_of_the_week", "monday,tuesday,wednesday,thursday,friday,saturday,sunday")
                .OldAnnotation("Npgsql:Enum:tracking.exam_status", "not_started,in_progress,finished,canceled");

            migrationBuilder.AlterColumn<string>(
                name: "exam_language",
                schema: "auth",
                table: "user_preferences",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "content.language");

            migrationBuilder.AlterColumn<string>(
                name: "color_scheme",
                schema: "auth",
                table: "user_preferences",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "auth.color_scheme");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                schema: "content",
                table: "subject",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "content.subject_name");

            migrationBuilder.AlterColumn<string>(
                name: "weekday",
                schema: "planning",
                table: "schedule_subject",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "planning.day_of_the_week");

            migrationBuilder.AlterColumn<string>(
                name: "language",
                schema: "content",
                table: "question",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "content.language",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "title",
                schema: "tracking",
                table: "exam",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "status",
                schema: "tracking",
                table: "exam",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "tracking.exam_status");

            migrationBuilder.AlterColumn<string>(
                name: "language_choice",
                schema: "tracking",
                table: "exam",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "content.language");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:auth.color_scheme", "os,dark,light")
                .Annotation("Npgsql:Enum:content.language", "INGLES,ESPANHOL")
                .Annotation("Npgsql:Enum:content.subject_name", "linguagens,ciencias-humanas,ciencias-natureza,matematica")
                .Annotation("Npgsql:Enum:planning.day_of_the_week", "monday,tuesday,wednesday,thursday,friday,saturday,sunday")
                .Annotation("Npgsql:Enum:tracking.exam_status", "not_started,in_progress,finished,canceled")
                .OldAnnotation("Npgsql:Enum:auth.color_scheme", "dark,light,os")
                .OldAnnotation("Npgsql:Enum:content.language", "ESPANHOL,INGLES")
                .OldAnnotation("Npgsql:Enum:content.subject_name", "ciencias-humanas,ciencias-natureza,linguagens,matematica")
                .OldAnnotation("Npgsql:Enum:planning.day_of_the_week", "friday,monday,saturday,sunday,thursday,tuesday,wednesday")
                .OldAnnotation("Npgsql:Enum:tracking.exam_status", "canceled,finished,in_progress,not_started");

            migrationBuilder.AlterColumn<string>(
                name: "exam_language",
                schema: "auth",
                table: "user_preferences",
                type: "content.language",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "color_scheme",
                schema: "auth",
                table: "user_preferences",
                type: "auth.color_scheme",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "name",
                schema: "content",
                table: "subject",
                type: "content.subject_name",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "weekday",
                schema: "planning",
                table: "schedule_subject",
                type: "planning.day_of_the_week",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "language",
                schema: "content",
                table: "question",
                type: "content.language",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "title",
                schema: "tracking",
                table: "exam",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<int>(
                name: "status",
                schema: "tracking",
                table: "exam",
                type: "tracking.exam_status",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "language_choice",
                schema: "tracking",
                table: "exam",
                type: "content.language",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
