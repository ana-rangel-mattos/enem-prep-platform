using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnemPrep.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_Has_Weekend_To_Schedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "has_week_end",
                schema: "planning",
                table: "schedule",
                type: "boolean",
                nullable: false,
                defaultValueSql: "FALSE");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "has_week_end",
                schema: "planning",
                table: "schedule");
        }
    }
}
