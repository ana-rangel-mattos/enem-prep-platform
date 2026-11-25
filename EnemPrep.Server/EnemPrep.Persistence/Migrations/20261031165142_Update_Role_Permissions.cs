using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EnemPrep.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Update_Role_Permissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "auth",
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 9, 3 });

            migrationBuilder.DeleteData(
                schema: "auth",
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 10, 3 });

            migrationBuilder.DeleteData(
                schema: "auth",
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 11, 3 });

            migrationBuilder.DeleteData(
                schema: "auth",
                table: "permissions",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                schema: "auth",
                table: "permissions",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                schema: "auth",
                table: "permissions",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DropColumn(
                name: "order_index",
                schema: "content",
                table: "playlist_question");

            migrationBuilder.InsertData(
                schema: "auth",
                table: "permissions",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 12, "ReadArticle" },
                    { 13, "ReadPlaylist" },
                    { 14, "CreatePlaylist" },
                    { 15, "DeletePlaylist" },
                    { 16, "EditPlaylist" }
                });

            migrationBuilder.InsertData(
                schema: "auth",
                table: "RolePermission",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { 12, 1 },
                    { 13, 1 },
                    { 14, 1 },
                    { 15, 1 },
                    { 16, 1 },
                    { 12, 3 },
                    { 13, 3 },
                    { 14, 3 },
                    { 15, 3 },
                    { 16, 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "auth",
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 12, 1 });

            migrationBuilder.DeleteData(
                schema: "auth",
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 13, 1 });

            migrationBuilder.DeleteData(
                schema: "auth",
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 14, 1 });

            migrationBuilder.DeleteData(
                schema: "auth",
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 15, 1 });

            migrationBuilder.DeleteData(
                schema: "auth",
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 16, 1 });

            migrationBuilder.DeleteData(
                schema: "auth",
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 12, 3 });

            migrationBuilder.DeleteData(
                schema: "auth",
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 13, 3 });

            migrationBuilder.DeleteData(
                schema: "auth",
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 14, 3 });

            migrationBuilder.DeleteData(
                schema: "auth",
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 15, 3 });

            migrationBuilder.DeleteData(
                schema: "auth",
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 16, 3 });

            migrationBuilder.DeleteData(
                schema: "auth",
                table: "permissions",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                schema: "auth",
                table: "permissions",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                schema: "auth",
                table: "permissions",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                schema: "auth",
                table: "permissions",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                schema: "auth",
                table: "permissions",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.AddColumn<int>(
                name: "order_index",
                schema: "content",
                table: "playlist_question",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                schema: "auth",
                table: "permissions",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 9, "CreateExerciseList" },
                    { 10, "DeleteExerciseList" },
                    { 11, "EditExerciseList" }
                });

            migrationBuilder.InsertData(
                schema: "auth",
                table: "RolePermission",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { 9, 3 },
                    { 10, 3 },
                    { 11, 3 }
                });
        }
    }
}
