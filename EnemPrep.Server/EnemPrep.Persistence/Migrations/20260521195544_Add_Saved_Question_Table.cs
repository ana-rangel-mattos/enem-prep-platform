using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnemPrep.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_Saved_Question_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "saved_question",
                schema: "auth",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    question_id = table.Column<Guid>(type: "uuid", nullable: false),
                    notes = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("saved_question_pkey", x => new { x.user_id, x.question_id });
                    table.ForeignKey(
                        name: "fk_question_id",
                        column: x => x.question_id,
                        principalSchema: "content",
                        principalTable: "question",
                        principalColumn: "question_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_user_id",
                        column: x => x.user_id,
                        principalSchema: "auth",
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_saved_question_question_id",
                schema: "auth",
                table: "saved_question",
                column: "question_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_user_id",
                schema: "tracking",
                table: "exam");

            migrationBuilder.DropTable(
                name: "saved_question",
                schema: "auth");
        }
    }
}
