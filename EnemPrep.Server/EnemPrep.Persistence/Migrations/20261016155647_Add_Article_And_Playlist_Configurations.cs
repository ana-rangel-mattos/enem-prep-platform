using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EnemPrep.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_Article_And_Playlist_Configurations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "article",
                schema: "content",
                columns: table => new
                {
                    article_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    subtitle = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    blocks_json = table.Column<string>(type: "jsonb", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("article_pkey", x => x.article_id);
                    table.ForeignKey(
                        name: "fk_user_id",
                        column: x => x.user_id,
                        principalSchema: "auth",
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "playlist",
                schema: "content",
                columns: table => new
                {
                    playlist_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    is_public = table.Column<bool>(type: "boolean", nullable: false, defaultValueSql: "TRUE"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("playlist_pkey", x => x.playlist_id);
                    table.ForeignKey(
                        name: "fk_user_id",
                        column: x => x.user_id,
                        principalSchema: "auth",
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "article_playlist",
                schema: "content",
                columns: table => new
                {
                    article_id = table.Column<Guid>(type: "uuid", nullable: false),
                    playlist_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("article_playlist_pk", x => new { x.article_id, x.playlist_id });
                    table.ForeignKey(
                        name: "article_playlist_article_id_fkey",
                        column: x => x.article_id,
                        principalSchema: "content",
                        principalTable: "article",
                        principalColumn: "article_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "article_playlist_playlist_id_fkey",
                        column: x => x.playlist_id,
                        principalSchema: "content",
                        principalTable: "playlist",
                        principalColumn: "playlist_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "playlist_question",
                schema: "content",
                columns: table => new
                {
                    playlist_id = table.Column<Guid>(type: "uuid", nullable: false),
                    question_id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_index = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("playlist_question_pk", x => new { x.playlist_id, x.question_id });
                    table.ForeignKey(
                        name: "playlist_question_playlist_id_fkey",
                        column: x => x.playlist_id,
                        principalSchema: "content",
                        principalTable: "playlist",
                        principalColumn: "playlist_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "playlist_question_question_id_fkey",
                        column: x => x.question_id,
                        principalSchema: "content",
                        principalTable: "question",
                        principalColumn: "question_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "auth",
                table: "permissions",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 6, "CreateArticle" },
                    { 7, "DeleteArticle" },
                    { 8, "EditArticle" },
                    { 9, "CreateExerciseList" },
                    { 10, "DeleteExerciseList" },
                    { 11, "EditExerciseList" }
                });

            migrationBuilder.InsertData(
                schema: "auth",
                table: "roles",
                columns: new[] { "Id", "Name" },
                values: new object[] { 3, "Teacher" });

            migrationBuilder.InsertData(
                schema: "auth",
                table: "RolePermission",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { 6, 3 },
                    { 7, 3 },
                    { 8, 3 },
                    { 9, 3 },
                    { 10, 3 },
                    { 11, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_article_user_id",
                schema: "content",
                table: "article",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_article_playlist_playlist_id",
                schema: "content",
                table: "article_playlist",
                column: "playlist_id");

            migrationBuilder.CreateIndex(
                name: "IX_playlist_user_id",
                schema: "content",
                table: "playlist",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_playlist_question_question_id",
                schema: "content",
                table: "playlist_question",
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
                name: "article_playlist",
                schema: "content");

            migrationBuilder.DropTable(
                name: "playlist_question",
                schema: "content");

            migrationBuilder.DropTable(
                name: "article",
                schema: "content");

            migrationBuilder.DropTable(
                name: "playlist",
                schema: "content");

            migrationBuilder.DeleteData(
                schema: "auth",
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 6, 3 });

            migrationBuilder.DeleteData(
                schema: "auth",
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 7, 3 });

            migrationBuilder.DeleteData(
                schema: "auth",
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 8, 3 });

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
                keyValue: 6);

            migrationBuilder.DeleteData(
                schema: "auth",
                table: "permissions",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                schema: "auth",
                table: "permissions",
                keyColumn: "Id",
                keyValue: 8);

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

            migrationBuilder.DeleteData(
                schema: "auth",
                table: "roles",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
