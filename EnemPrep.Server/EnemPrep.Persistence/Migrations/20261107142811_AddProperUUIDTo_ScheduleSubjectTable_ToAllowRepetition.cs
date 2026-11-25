using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnemPrep.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddProperUUIDTo_ScheduleSubjectTable_ToAllowRepetition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_schedule_subject",
                schema: "planning",
                table: "schedule_subject");

            migrationBuilder.AddColumn<Guid>(
                name: "schedule_subject_id",
                schema: "planning",
                table: "schedule_subject",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "pk_schedule_subject",
                schema: "planning",
                table: "schedule_subject",
                column: "schedule_subject_id");

            migrationBuilder.CreateIndex(
                name: "IX_schedule_subject_schedule_id",
                schema: "planning",
                table: "schedule_subject",
                column: "schedule_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_schedule_subject",
                schema: "planning",
                table: "schedule_subject");

            migrationBuilder.DropIndex(
                name: "IX_schedule_subject_schedule_id",
                schema: "planning",
                table: "schedule_subject");

            migrationBuilder.DropColumn(
                name: "schedule_subject_id",
                schema: "planning",
                table: "schedule_subject");

            migrationBuilder.AddPrimaryKey(
                name: "pk_schedule_subject",
                schema: "planning",
                table: "schedule_subject",
                columns: new[] { "schedule_id", "subject_id" });
        }
    }
}
