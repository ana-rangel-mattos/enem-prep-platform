using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnemPrep.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Set_Language_As_Optional_On_Exam_Model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "language_choice",
                schema: "tracking",
                table: "exam",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "language_choice",
                schema: "tracking",
                table: "exam",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
