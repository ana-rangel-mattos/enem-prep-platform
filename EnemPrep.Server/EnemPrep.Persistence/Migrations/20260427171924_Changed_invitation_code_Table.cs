using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnemPrep.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Changed_invitation_code_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                schema: "auth",
                table: "invitation_code",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_invitation_code_RoleId",
                schema: "auth",
                table: "invitation_code",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "fk_role_id",
                schema: "auth",
                table: "invitation_code",
                column: "RoleId",
                principalSchema: "auth",
                principalTable: "roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_role_id",
                schema: "auth",
                table: "invitation_code");

            migrationBuilder.DropIndex(
                name: "IX_invitation_code_RoleId",
                schema: "auth",
                table: "invitation_code");

            migrationBuilder.DropColumn(
                name: "RoleId",
                schema: "auth",
                table: "invitation_code");
        }
    }
}
