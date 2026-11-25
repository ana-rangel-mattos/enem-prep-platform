using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EnemPrep.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_Subject_Permissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "auth",
                table: "permissions",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 17, "CreateSubject" },
                    { 18, "DeleteSubject" }
                });

            migrationBuilder.InsertData(
                schema: "auth",
                table: "RolePermission",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { 17, 2 },
                    { 18, 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "auth",
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 17, 2 });

            migrationBuilder.DeleteData(
                schema: "auth",
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 18, 2 });

            migrationBuilder.DeleteData(
                schema: "auth",
                table: "permissions",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                schema: "auth",
                table: "permissions",
                keyColumn: "Id",
                keyValue: 18);
        }
    }
}
