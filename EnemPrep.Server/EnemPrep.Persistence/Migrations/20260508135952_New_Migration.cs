using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnemPrep.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class New_Migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:auth.color_scheme", "os,dark,light")
                .Annotation("Npgsql:Enum:content.language", "INGLES,ESPANHOL")
                .Annotation("Npgsql:Enum:content.subject_name", "linguagens,ciencias-humanas,ciencias-natureza,matematica")
                .Annotation("Npgsql:Enum:planning.day_of_the_week", "monday,tuesday,wednesday,thursday,friday,saturday,sunday")
                .Annotation("Npgsql:Enum:tracking.exam_status", "not_started,in_progress,finished,canceled")
                .OldAnnotation("Npgsql:Enum:auth.color_scheme", "os,dark,light")
                .OldAnnotation("Npgsql:Enum:auth.user_role", "STUDENT,ADMIN")
                .OldAnnotation("Npgsql:Enum:content.language", "INGLES,ESPANHOL")
                .OldAnnotation("Npgsql:Enum:content.subject_name", "linguagens,ciencias-humanas,ciencias-natureza,matematica")
                .OldAnnotation("Npgsql:Enum:planning.day_of_the_week", "monday,tuesday,wednesday,thursday,friday,saturday,sunday")
                .OldAnnotation("Npgsql:Enum:tracking.exam_status", "not_started,in_progress,finished,canceled");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:auth.color_scheme", "os,dark,light")
                .Annotation("Npgsql:Enum:auth.user_role", "STUDENT,ADMIN")
                .Annotation("Npgsql:Enum:content.language", "INGLES,ESPANHOL")
                .Annotation("Npgsql:Enum:content.subject_name", "linguagens,ciencias-humanas,ciencias-natureza,matematica")
                .Annotation("Npgsql:Enum:planning.day_of_the_week", "monday,tuesday,wednesday,thursday,friday,saturday,sunday")
                .Annotation("Npgsql:Enum:tracking.exam_status", "not_started,in_progress,finished,canceled")
                .OldAnnotation("Npgsql:Enum:auth.color_scheme", "os,dark,light")
                .OldAnnotation("Npgsql:Enum:content.language", "INGLES,ESPANHOL")
                .OldAnnotation("Npgsql:Enum:content.subject_name", "linguagens,ciencias-humanas,ciencias-natureza,matematica")
                .OldAnnotation("Npgsql:Enum:planning.day_of_the_week", "monday,tuesday,wednesday,thursday,friday,saturday,sunday")
                .OldAnnotation("Npgsql:Enum:tracking.exam_status", "not_started,in_progress,finished,canceled");
        }
    }
}
